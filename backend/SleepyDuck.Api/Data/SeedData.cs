using System.Security.Cryptography;
using System.Text;
using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Data;

/// <summary>
/// IT support troubleshooting deck. EF Core data seeding needs stable keys, so
/// ids are hashed from each item's position; editing text won't churn the migration.
/// </summary>
public static class SeedData
{
    private sealed record Wrong(string Text, string Why);

    private sealed record Step(string Prompt, string Correct, string Why, params Wrong[] Distractors);

    private sealed record Scenario(
        string Key,
        ScenarioCategory Category,
        string Title,
        string Problem,
        string Technical,
        params Step[] Steps);

    public static IReadOnlyList<ScenarioCard> Cards { get; }
    public static IReadOnlyList<SequentialStep> Steps { get; }
    public static IReadOnlyList<Distractor> Distractors { get; }

    static SeedData()
    {
        var cards = new List<ScenarioCard>();
        var steps = new List<SequentialStep>();
        var distractors = new List<Distractor>();

        foreach (var scenario in Scenarios)
        {
            var cardId = StableId(scenario.Key);
            cards.Add(new ScenarioCard
            {
                Id = cardId,
                Title = scenario.Title,
                Category = scenario.Category,
                ProblemStatement = scenario.Problem,
                TechnicalExplanation = scenario.Technical,
            });

            foreach (var (step, stepIndex) in scenario.Steps.Select((s, i) => (s, i + 1)))
            {
                var stepKey = $"{scenario.Key}/step-{stepIndex}";
                var stepId = StableId(stepKey);
                steps.Add(new SequentialStep
                {
                    Id = stepId,
                    ScenarioCardId = cardId,
                    Order = stepIndex,
                    Prompt = step.Prompt,
                    CorrectOptionId = StableId($"{stepKey}/correct"),
                    CorrectAction = step.Correct,
                    Explanation = step.Why,
                });

                foreach (var (wrong, wrongIndex) in step.Distractors.Select((d, i) => (d, i + 1)))
                {
                    distractors.Add(new Distractor
                    {
                        Id = StableId($"{stepKey}/distractor-{wrongIndex}"),
                        SequentialStepId = stepId,
                        Text = wrong.Text,
                        Explanation = wrong.Why,
                    });
                }
            }
        }

        Cards = cards;
        Steps = steps;
        Distractors = distractors;
    }

    private static Guid StableId(string key) => new(MD5.HashData(Encoding.UTF8.GetBytes(key)));

    private static readonly Scenario[] Scenarios =
    [
        // ---------------------------------------------------------------- Email delivery
        new(
            "email-external-not-received",
            ScenarioCategory.EmailDelivery,
            "External emails never arrive",
            "Priya in Finance says invoices from a supplier at billing@northwind-supply.example have stopped reaching her. Internal email works, and she has checked her Junk Email folder. The supplier received no bounce message.",
            "Message trace is the starting point for any 'where did my email go?' ticket because it shows what Exchange Online actually did with the message. A Quarantined status means Microsoft Defender for Office 365 or Exchange Online Protection held the message before it reached the mailbox, which is why it was not in Junk and why the sender got no bounce. After releasing it, report the false positive through Submissions and let Microsoft create the Tenant Allow/Block List entry. That keeps the exception narrow and time-limited, whereas a domain on the anti-spam allowed list skips spam filtering for anyone spoofing that domain.",
            new Step(
                "Nothing on the client looks wrong. Where should you start?",
                "Open the Exchange admin center and go to Mail flow > Message trace",
                "Internal mail works and nothing is in Junk, so Outlook and the mailbox are fine and the message was stopped before delivery. Message trace is Exchange Online's record of every message it handled. It shows whether a message was received, and whether it was Delivered, Quarantined, Failed or still Pending. If the trace finds nothing at all, the message never reached Microsoft 365 and the problem is on the sender's side.",
                new Wrong("Rebuild Priya's Outlook profile", "Her mail client isn't implicated. Internal mail works, and a new profile can't reveal what happened to a message that never reached the mailbox."),
                new Wrong("Reset Priya's password and revoke her sessions", "Nothing suggests a compromised account, and sign-in state doesn't affect inbound delivery."),
                new Wrong("Ask the supplier to resend the invoice from a personal mailbox, so that Priya at least gets it today", "This sidesteps the problem without finding the cause, and it teaches staff to accept invoices from unfamiliar addresses.")),
            new Step(
                "Message trace is open. How do you scope the search?",
                "Set the sender to billing@northwind-supply.example, the recipient to Priya, and a date range covering the missing invoices",
                "A trace needs the sender, the recipient and a date range. Using both addresses returns only the supplier's messages to Priya, and the window has to reach back to the first missing invoice. Searches of the last 10 days come back within moments, and older ones run as a downloadable report, up to 90 days back. Each result then shows a status and the events behind it.",
                new Wrong("Search with Priya as the sender over the last 24 hours", "That finds what Priya sent, not what she should have received."),
                new Wrong("Run an unfiltered trace for the whole organization", "It returns a large volume of unrelated results and may hide the message you need."),
                new Wrong("Run a content search for 'invoice' across Priya's mailbox in Microsoft Purview, using the supplier's domain and a date range covering the missing invoices", "Content search only looks at items stored in mailboxes and other locations, so it can't find a message that was never delivered.")),
            new Step(
                "The trace shows the supplier's messages with the status Quarantined, flagged as high confidence spam. What is your next step?",
                "Open Quarantine in the Microsoft Defender portal, review the messages, and release them if they are legitimate",
                "Quarantined means Exchange Online Protection or Defender for Office 365 judged the message unsafe and held it, so it never reached any mailbox folder and the sender got no bounce. High confidence spam is the strongest spam verdict, an SCL (spam confidence level) of 9. In the Defender portal, go to Email & collaboration > Review > Quarantine. Check the sender, the authentication results and a preview before releasing, because an invoice is exactly what a phishing email imitates.",
                new Wrong("Tell Priya to look in her Deleted Items folder", "A quarantined message was never delivered to a mailbox folder, so it won't be there."),
                new Wrong("Disable the anti-spam policy for the tenant", "This removes protection for everyone to fix a false positive affecting a single sender."),
                new Wrong("Ask the supplier to resend the invoices, then wait for the new copies to clear filtering and confirm that Priya has them", "The same filter verdict will probably apply again, and the held invoices remain stuck.")),
            new Step(
                "The messages were legitimate. How do you stop this from recurring?",
                "Report them as false positives through Submissions in the Defender portal and create a Tenant Allow/Block List allow entry from the submission",
                "Releasing fixes today's messages only. A submission marked as clean sends the message to Microsoft for analysis, and it can create an allow entry in the Tenant Allow/Block List at the same time. That entry covers this sender only and expires, by default 45 days after it was last used. The broad shortcuts are riskier, because they skip filtering for anyone who forges the supplier's address.",
                new Wrong("Add northwind-supply.example to the allowed domains in the anti-spam policy that covers Priya, so future mail from the supplier isn't filtered as spam again", "Allowed domains are a known risk because spoofed mail from the domain can bypass spam filtering."),
                new Wrong("Create a mail flow rule that sets SCL to -1 for all external mail", "This bypasses spam filtering for all inbound mail, not just this supplier."),
                new Wrong("Add the supplier to Priya's Safe Senders in Outlook", "This doesn't reliably affect high confidence spam verdicts, and it helps only one mailbox."))),

        new(
            "email-forwarding-blocked",
            ScenarioCategory.EmailDelivery,
            "Auto-forward bounces with 5.7.520",
            "Marco, a contractor manager, set an inbox rule to forward project mail to a partner's external address. Every forwarded message now bounces, and the bounce message includes the code 5.7.520.",
            "NDR (non-delivery report) 5.7.520 means 'Access denied, your organization does not allow external forwarding'. Automatic external forwarding is controlled by the outbound spam filter policy, whose default setting (Automatic - System-controlled) behaves as Off. Attackers often use forwarding rules to exfiltrate mail from compromised mailboxes, so leave the Default policy alone and enable forwarding only through a custom policy scoped to the people with an approved business need. Check any remote domain settings as well, because they can also stop automatic forwarding.",
            new Step(
                "Before changing anything, what does the code 5.7.520 tell you?",
                "Automatic forwarding to an external recipient was blocked by your own organization's policy",
                "An NDR (non-delivery report), or bounce message, carries an enhanced status code. 5.x.x is a permanent failure, and 5.7.x is a security or policy block. 5.7.520 comes with the text 'Access denied, your organization does not allow external forwarding'. So Exchange Online in your own tenant refused the message before it left, and nothing at the partner is at fault. Reading the code first tells you which side to investigate.",
                new Wrong("The partner's mail server rejected Marco's domain, which is why the bounce came back with a 5.7 code", "The block happens on your side of the connection before the message leaves Exchange Online."),
                new Wrong("Marco's mailbox has gone over its send quota", "Exceeding a quota produces different NDR codes and affects normal sending as well."),
                new Wrong("The domain's SPF (Sender Policy Framework) record is failing", "SPF failures are reported by the receiving system and have nothing to do with a forwarding policy.")),
            new Step(
                "Which setting controls this behavior?",
                "The automatic forwarding setting in the outbound anti-spam policy in the Microsoft Defender portal",
                "Automatic external forwarding is governed by outbound spam filter policies, under Email & collaboration > Policies & rules > Threat policies > Anti-spam in the Defender portal. The setting has three values: Automatic - System-controlled, On, and Off. The default, Automatic, behaves as Off, which is why a rule that a user is able to create still gets blocked. It's off by default because attackers who take over a mailbox often add a forwarding rule to copy mail out.",
                new Wrong("The Conditional Access policy that applies to Marco", "Conditional Access evaluates sign-ins, not how Exchange routes mail."),
                new Wrong("The inbound anti-spam policy", "Inbound policies handle mail arriving in the tenant. Forwarding is an outbound control."),
                new Wrong("Marco's Outlook rules and alerts settings", "The rule itself is working. It's being blocked by tenant policy after it fires.")),
            new Step(
                "The business has approved this forwarding for Marco. How should you apply the change?",
                "Create a custom outbound policy with forwarding set to On and scope it to Marco",
                "Policies are scoped to users, groups or domains, and a custom policy takes precedence over the Default one. A custom outbound policy with forwarding set to On, applied only to Marco, allows the approved exception, and everyone else keeps the blocked default. Record who approved it, because each mailbox that can forward externally is one an attacker could use to leak mail.",
                new Wrong("Set automatic forwarding to On in the Default outbound policy, since that's the policy the mailbox is using today", "That permits external forwarding for all users, including any compromised account."),
                new Wrong("Give Marco the Global Administrator role", "Admin roles don't exempt a user from mail policy, and this grossly over-privileges the account."),
                new Wrong("Remove the outbound policy", "The Default policy can't be deleted, and removing protection wouldn't be the right fix anyway.")),
            new Step(
                "How do you confirm the fix?",
                "Send a test message that triggers the rule, then confirm delivery with a message trace",
                "Policy changes in Microsoft 365 aren't instant, so wait before judging the result. Then test the real path. Send Marco a message that matches his rule, and run a message trace on it. The trace should show the original delivery and a second, forwarded message to the partner's address, with the status Delivered and no 5.7.520.",
                new Wrong("Ask the partner to mark Marco's address as safe", "The problem was never on the partner's side."),
                new Wrong("Close the ticket as soon as the policy is saved", "A saved setting isn't proof, particularly when policies take time to apply."),
                new Wrong("Check Marco's Sent Items for the forwarded copy after the next matching message triggers the rule", "Server-processed forwards typically don't appear in Sent Items, so this proves nothing."))),

        new(
            "exo-powershell-hidden-inbox-rule",
            ScenarioCategory.EmailDelivery,
            "Hidden inbox rule found with PowerShell",
            "Grace, the CFO's assistant, says replies from the bank about a pending wire transfer never arrive, but the bank insists it sent them. Message trace shows every reply as Delivered to Grace's mailbox. Security wants the ticket worked entirely from PowerShell so that every action is scripted and recorded.",
            "When message trace says Delivered but the user never sees the message, something inside the mailbox moved or deleted it after delivery. In business email compromise, attackers commonly create inbox rules that move mail matching words such as 'bank', 'wire' or 'invoice' to a folder nobody checks, and these rules are often hidden from Outlook. Get-InboxRule with -IncludeHidden returns rules that Outlook doesn't show, and Get-Mailbox reveals mailbox-level forwarding set through ForwardingSmtpAddress. Removing the rule only fixes the symptom. The account has to be treated as compromised: reset the password and revoke its sessions so existing refresh tokens stop working, or the attacker can simply create the rule again. Connect with the Exchange Online PowerShell V3 module, because Basic authentication for remote PowerShell is turned off and the MSOnline module is retired.",
            new Step(
                "How do you start the PowerShell session?",
                "Connect-ExchangeOnline -UserPrincipalName admin@contoso.example",
                "Connect-ExchangeOnline comes from the ExchangeOnlineManagement module, which you install once with Install-Module ExchangeOnlineManagement. It signs in with modern authentication, so MFA (multi-factor authentication) works, and it loads the Exchange cmdlets such as Get-InboxRule and Set-Mailbox. The older ways in are gone. Basic authentication for remote PowerShell is switched off, and the MSOnline module is retired.",
                new Wrong("Connect-MsolService -Credential (Get-Credential)", "The MSOnline module is retired, and it never managed mailboxes or inbox rules."),
                new Wrong("New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell-liveid/ -Authentication Basic", "Basic authentication for remote PowerShell has been turned off in Exchange Online, so this connection fails."),
                new Wrong("Connect-MgGraph -Scopes 'Mail.ReadWrite', then run Get-InboxRule", "Microsoft Graph PowerShell doesn't load Exchange cmdlets, so Get-InboxRule isn't available in that session.")),
            new Step(
                "Trace already shows Delivered. Which command shows what happens to the mail inside the mailbox?",
                "Get-InboxRule -Mailbox grace@contoso.example -IncludeHidden | Format-List Name,Description",
                "Message trace only covers transport, and Delivered means the message reached the mailbox. Whatever happened next happened inside the mailbox, and inbox rules are the usual cause. Get-InboxRule lists them, and -IncludeHidden adds the rules that Outlook doesn't show, which attackers create on purpose. The Description property reads out each rule's conditions and actions in plain words. Also run Get-Mailbox and look at ForwardingSmtpAddress, the other common place to hide a redirect.",
                new Wrong("Get-MessageTraceV2 -RecipientAddress grace@contoso.example -StartDate (Get-Date).AddDays(-7) -EndDate (Get-Date)", "Trace records what transport did, and it already shows Delivered. It can't see what happened to a message after it reached the mailbox."),
                new Wrong("Get-MailboxStatistics -Identity grace@contoso.example", "This reports item counts and mailbox size, not why particular messages disappear."),
                new Wrong("Get-MailboxFolderPermission -Identity grace@contoso.example:\\Inbox", "Folder permissions show who can open the Inbox. They don't show rules that move mail out of it.")),
            new Step(
                "The output shows a hidden rule named '.' that moves messages containing 'bank' or 'wire' to RSS Subscriptions and marks them as read. What does this indicate?",
                "The mailbox is compromised, and the attacker's rule hides the bank's replies (business email compromise)",
                "Each detail fits business email compromise. A name like '.' is hard to notice, the rule is hidden from Outlook, the keywords match the wire transfer, and RSS Subscriptions is a folder nobody opens. Marking the mail as read hides the unread count too. The purpose is to keep Grace from seeing the bank's replies while the attacker answers them, usually to change the payment details. This is now a security incident, not a mail flow ticket.",
                new Wrong("Grace created a cleanup rule by mistake and forgot about it", "Outlook doesn't create hidden rules, and the keywords target exactly the wire transfer conversation."),
                new Wrong("A Defender for Office 365 policy moved the messages as suspected phishing", "Defender actions show up in message trace and quarantine, not as an inbox rule in the user's mailbox."),
                new Wrong("An Outlook sync problem on Grace's laptop is filing new mail into RSS Subscriptions, so the messages land there automatically", "A client sync fault doesn't create a server-side rule with keyword conditions.")),
            new Step(
                "How do you contain this from PowerShell?",
                "Remove the rule, clear any mailbox forwarding with Set-Mailbox, reset the password, and run Revoke-MgUserSignInSession",
                "Containment has two halves. Remove-InboxRule and Set-Mailbox with -ForwardingSmtpAddress $null undo what the attacker set up. Resetting the password and running Revoke-MgUserSignInSession remove the attacker's access, because revoking invalidates the refresh tokens that would otherwise keep working after a password change. Doing only the first half lets the attacker create the rule again. After that, review the sign-in logs and MFA methods, and warn Finance about the wire transfer.",
                new Wrong("Disable the rule with Disable-InboxRule so it can be turned back on later if Grace says it was intentional", "The attacker keeps a valid session and can recreate or re-enable the rule at any time."),
                new Wrong("Set-Mailbox -Identity grace@contoso.example -HiddenFromAddressListsEnabled $true", "Hiding the mailbox from the address book doesn't touch the rule or the attacker's access."),
                new Wrong("Delete the rule with Remove-InboxRule, then ask Grace to watch the Inbox closely for the rest of the week and report anything else that goes missing", "Deleting the rule alone leaves the attacker signed in with a valid token, free to create it again."))),

        // ------------------------------------------------------- User & license management
        new(
            "user-e3-restricted-apps",
            ScenarioCategory.UserLicenseManagement,
            "New hire on E3 with restricted apps",
            "Dana joins the Warehouse team on Monday. They need a Microsoft 365 E3 license, but HR policy says warehouse staff shouldn't have access to SharePoint or Viva Engage.",
            "A Microsoft 365 license bundles many service plans, and each plan can be switched off individually. Usage location must be set first because Microsoft needs it to determine which services are available in a country or region. Turning off apps in the license removes the entitlement to those services while leaving the rest of the E3 subscription intact. Conditional Access solves a different problem, since it controls whether a sign-in is allowed and doesn't change what the user is licensed for.",
            new Step(
                "Where do you create Dana's account?",
                "Microsoft 365 admin center > Users > Active users > Add a user",
                "Active users > Add a user creates the Entra ID account and walks through the basics, then the usage location and license, then optional roles and profile details, in one pass. The mailbox and other services are provisioned once the license is assigned. For many users at once you'd use a CSV (comma-separated values) import or PowerShell, but for one new hire the wizard is the standard route.",
                new Wrong("Exchange admin center > Recipients > Contacts > Add a mail contact", "A mail contact is an external address entry with no sign-in or license."),
                new Wrong("Teams admin center > Users", "This manages Teams settings for existing users. It doesn't create accounts."),
                new Wrong("Microsoft Purview > Roles & scopes", "Purview roles govern compliance permissions, not user provisioning.")),
            new Step(
                "The wizard reaches product licenses. What must be set before the E3 license can be assigned?",
                "The usage location, a country or region set on the user account",
                "Not every Microsoft 365 service is available in every country, so the usage location tells Microsoft which services it can provide to this user. Assigning a license without one fails. It's a property of each user, and a missing value is the most common reason a license assignment fails, including with group-based licensing.",
                new Wrong("A manager in Microsoft Entra ID", "A manager is useful directory data but it isn't a licensing requirement."),
                new Wrong("A completed MFA (multi-factor authentication) registration", "MFA is registered by the user later and doesn't block license assignment."),
                new Wrong("A department attribute", "Department is optional unless your own automation depends on it.")),
            new Step(
                "How do you keep Dana from getting SharePoint and Viva Engage while keeping the rest of E3?",
                "Select the E3 license, expand Apps, and turn off the SharePoint and Viva Engage service plans",
                "A license such as E3 is a bundle of service plans: Exchange Online, SharePoint, Teams, the Office apps, Viva Engage and others. In the wizard, or later on the user's Licenses and apps tab, the Apps list lets you clear individual plans. Turning a plan off removes Dana's entitlement to that service and leaves the rest of E3 working. One thing to know is that OneDrive depends on the SharePoint plan, so turning SharePoint off removes OneDrive too.",
                new Wrong("Block Dana from SharePoint and Viva Engage with a Conditional Access policy, and leave the full E3 license in place", "Conditional Access blocks sign-ins to apps, but Dana would still be licensed and it doesn't change the entitlement HR asked about."),
                new Wrong("Assign Exchange Online Plan 1 instead of E3", "This loses the rest of E3, such as the Office apps, which Dana is meant to have."),
                new Wrong("Delete the SharePoint site collection for the Warehouse team", "This destroys content for others and doesn't affect Dana's entitlement.")),
            new Step(
                "How do you verify the result before Monday?",
                "Open Dana's user pane and check Licenses and apps to confirm that those two apps are off",
                "Verify what was actually saved, before the user arrives. The user's Licenses and apps tab shows the assigned license and the state of each service plan, so you can see E3 assigned with SharePoint and Viva Engage cleared. Other admin pages show tenant totals or a single service, which doesn't answer the HR requirement.",
                new Wrong("Check the Billing > Your products page", "It shows tenant-level subscriptions and license counts, not one user's service plans."),
                new Wrong("Ask Dana to sign in and report back on Monday", "That's too late, and it relies on the user to discover a misconfiguration."),
                new Wrong("Review the Exchange admin center for the mailbox, then confirm that its settings match what HR asked for", "This confirms that a mailbox exists but says nothing about SharePoint or Viva Engage."))),

        new(
            "user-group-based-licensing",
            ScenarioCategory.UserLicenseManagement,
            "Licensing a whole department at scale",
            "The 50-person Marketing department needs E3 without Viva Engage. People join and leave constantly, and assigning licenses by hand has been causing mistakes. After you make the change, a few users show a licensing error.",
            "Group-based licensing in Microsoft Entra ID assigns a license to every member of a group, so a dynamic or well-managed group handles joiners and leavers automatically. Each group assignment can also disable service plans. Group-based licensing requires Microsoft Entra ID P1, which Microsoft 365 E3 includes. Assignment errors don't stop the rest of the group from being processed. They appear per user on the group's license page, and the most common causes are a missing usage location, not enough available licenses, and conflicting service plans.",
            new Step(
                "What is the best way to reduce the manual licensing work?",
                "Use group-based licensing in Microsoft Entra ID on a Marketing security group",
                "With group-based licensing you assign the license to a group once, and Entra ID gives it to every member. A new hire who is added to the group gets E3 automatically, and someone who is removed loses it and the license returns to the pool. It needs a security group or a Microsoft 365 group, and Entra ID P1, which E3 includes. If membership is dynamic, based on the department attribute, even adding people to the group is automatic.",
                new Wrong("Bulk-select all 50 users in Active users once", "That fixes today's list, but the next joiner has to be handled by hand again."),
                new Wrong("Add a mail flow rule for the Marketing department", "Mail flow rules process messages. They can't assign licenses."),
                new Wrong("Assign the license to a distribution list that HR keeps up to date for the Marketing department", "Group-based licensing needs a security-enabled group or Microsoft 365 group. A distribution list isn't supported.")),
            new Step(
                "When you assign E3 to the group, how do you exclude Viva Engage?",
                "Turn off the Viva Engage service plan in the group's license assignment options",
                "The service plan switches that exist for a single user also exist on a group's license assignment. Clearing Viva Engage there applies to every current and future member. If a user holds the same license from two places, the enabled plans are combined, so a plan only stays off if every assignment leaves it off.",
                new Wrong("Add a second group with Viva Engage turned off, and put the same Marketing users in both groups", "Assignments are additive. A second group can't subtract what the first one grants."),
                new Wrong("Remove Viva Engage from each user afterward", "Manual per-user edits are the work you're trying to eliminate, and each joiner would need it too."),
                new Wrong("Disable Viva Engage for the tenant", "This affects every department, not just Marketing.")),
            new Step(
                "Three users show a license error. Where do you find out why?",
                "On the group's Licenses page in Microsoft Entra ID, review the users with assignment errors",
                "Group licensing is processed in the background, one user at a time, and a user who fails doesn't stop the others. The group's Licenses page shows how many members are in an error state, and opening it lists each user with a reason, such as a missing usage location, not enough licenses, or conflicting service plans. The same information is in the Entra audit logs.",
                new Wrong("In the Exchange message trace", "Message trace records mail flow, not licensing."),
                new Wrong("In the Sign-in logs for those users", "Sign-in logs show authentication attempts, not license processing."),
                new Wrong("In Microsoft Purview Audit, by searching for failed logons from those three users since the assignment ran", "Failed logons aren't related to a license assignment error.")),
            new Step(
                "The error says the users have no usage location. How do you fix it?",
                "Set the usage location on those users, then reprocess the assignment if it doesn't retry by itself",
                "The error names the missing property, so fix that property and nothing else. Set the usage location on the three users, and Entra ID normally retries the assignment by itself. The Reprocess button on the group's Licenses page forces it straight away. To prevent it, set the usage location when users are created, or sync it from HR data.",
                new Wrong("Remove them from the group permanently", "They still need the license, so this just abandons the requirement."),
                new Wrong("Purchase three additional E3 licenses", "The cause is a missing attribute, not a shortage of licenses."),
                new Wrong("Change the tenant's default language", "Language settings have no effect on usage location."))),

        new(
            "entra-ca-travel-block",
            ScenarioCategory.UserLicenseManagement,
            "Conditional Access blocks a traveling user",
            "Luis from Sales is at a client site in Lisbon and can't sign in to Outlook or Teams. The browser shows error AADSTS53003, 'Access has been blocked by Conditional Access policies'. No MFA (multi-factor authentication) prompt appears, and the same account worked from the office yesterday.",
            "Microsoft Entra ID evaluates every sign-in against the Conditional Access policies that apply to it, and the sign-in log records the result of each policy. AADSTS53003 means a policy's grant control was Block, which is why no MFA (multi-factor authentication) prompt appeared: a block is applied before any challenge is offered. Country-based named locations are resolved from the sign-in's public IP (Internet Protocol) address, so a user abroad falls outside an 'allowed countries' location. The supported way to make an exception is an exclusion group with a documented end date, ideally governed by an access review. Disabling the policy, switching it to Report-only, or trusting a hotel IP weakens protection for everyone. The What If tool evaluates a simulated sign-in against every policy, so you can prove the exception works before the user tries again.",
            new Step(
                "Where do you find out why the sign-in was blocked?",
                "Open Luis's failed sign-in in the Entra sign-in logs and review its Conditional Access tab",
                "Every sign-in attempt is logged in Entra ID under Sign-in logs, including failed ones. The failed entry gives the error code, the IP (Internet Protocol) address and the location it mapped to, the app, and the device. Its Conditional Access tab lists every policy that was evaluated and whether each one succeeded, failed or didn't apply. That names the policy that blocked Luis, so you don't have to guess.",
                new Wrong("Require Luis to re-register MFA from the Authentication methods page", "No MFA prompt was shown, so the methods were never used. Re-registering removes working methods without touching the block."),
                new Wrong("Check that Luis still has an E3 license assigned", "A missing license causes service errors after sign-in, not a Conditional Access block during it."),
                new Wrong("Search the Microsoft Purview audit log for Luis's mailbox activity since the last successful sign-in yesterday", "Mailbox auditing records actions inside the mailbox. It doesn't hold Conditional Access results.")),
            new Step(
                "The log shows error 53003, location Portugal, and the policy 'Block sign-ins outside allowed countries' with the result Failure. What is the cause?",
                "The policy blocked the sign-in because Portugal isn't in the allowed countries named location",
                "A Conditional Access policy is an if-then rule: if these users, apps and conditions match, then block, or grant access with requirements such as MFA. This policy's condition is any location outside the 'allowed countries' named location, and its control is Block. Entra works out the country from the public IP address of the sign-in. Luis's IP is Portuguese, so the condition matched. A block ends the sign-in at once, which is why he was never asked for MFA.",
                new Wrong("Luis failed the MFA challenge too many times and the account was locked out", "MFA failures return codes such as 500121, and a lockout returns 50053. The log shows a policy block."),
                new Wrong("Luis's password expired while traveling", "An expired password returns 50055 and prompts for a change. It isn't a Conditional Access result."),
                new Wrong("The hotel Wi-Fi is blocking traffic to login.microsoftonline.com, so the token request never completes", "The request reached Entra ID and was evaluated, because the sign-in log has an entry with a policy result.")),
            new Step(
                "Management approves access for the two-week trip. How do you apply it?",
                "Add Luis to a travel exclusion group that the policy excludes, with a set removal date",
                "Policies have Include and Exclude lists, and Exclude wins. Putting Luis in a group that the policy excludes lets only him through, and only while he's in the group. Set a removal date, or use an access review, so the exception doesn't become permanent. The other options all change the policy for everybody.",
                new Wrong("Switch the policy to Report-only mode until Luis returns", "Report-only stops enforcement for every user in scope, not just Luis."),
                new Wrong("Add the hotel's public IP address to a trusted named location", "The IP can change, and trusting it opens the policy to anyone on that network, including attackers."),
                new Wrong("Add Portugal to the allowed countries location in the policy, then remove it when the trip ends", "That allows sign-ins from Portugal for the entire organization, and removing it later is easy to forget.")),
            new Step(
                "How do you confirm the exception before Luis tries again?",
                "Use What If for Luis with Portugal as the country, and check the block policy isn't applied",
                "What If is a tool on the Conditional Access page. You pick a user, an app and conditions such as country or IP address, and it lists which policies would apply and which wouldn't. Running it for Luis from Portugal should show the block policy among the policies that will not apply, because of the exclusion. That proves the change before he tries. His next real sign-in in the sign-in logs is the final confirmation.",
                new Wrong("Ask Luis to clear the browser cache and try again in an hour", "Clearing the cache doesn't change policy evaluation, and waiting without testing proves nothing."),
                new Wrong("Check the policy's Last modified date and audit log entry to make sure the change was saved correctly", "A saved change proves the policy was edited, not that this user's sign-in will pass."),
                new Wrong("Sign in with a test account from the office network", "The office is inside the allowed countries, so this test can't reproduce Luis's situation."))),

        // --------------------------------------------------------------- Group management
        new(
            "group-m365-vs-dl",
            ScenarioCategory.GroupManagement,
            "Microsoft 365 Group or Distribution List?",
            "The Events team asks for 'a group'. They want a shared email address, a shared calendar, a place to store files, and a task board. Some partners outside the company also need to be able to email the address.",
            "A distribution list only delivers email to its members. A Microsoft 365 Group provisions a set of connected resources, including a group mailbox, calendar, SharePoint team site, and Planner plan, so it matches a team that collaborates. Microsoft 365 Groups don't accept mail from outside the organization by default, so this setting has to be turned on when external senders need to reach the group. Choosing a type up front matters, since it determines which workloads the team can use.",
            new Step(
                "Which group type fits the request?",
                "Microsoft 365 Group",
                "Match the group type to what the team needs. A distribution list is only an address that delivers mail to its members. A mail-enabled security group does that and can also be used to grant permissions. A Microsoft 365 Group creates a shared mailbox and calendar, a SharePoint site for files, and a Planner plan, all with one membership. The Events team asked for all four, so only the Microsoft 365 Group fits.",
                new Wrong("Distribution list", "A distribution list only sends email to its members, with no calendar, files, or tasks."),
                new Wrong("Mail-enabled security group", "This combines email delivery with access permissions but offers no collaboration workspace."),
                new Wrong("Dynamic distribution group", "Membership is automatic, but it's still email-only.")),
            new Step(
                "Where do you create it?",
                "Microsoft 365 admin center > Teams & groups > Active teams & groups, then add a Microsoft 365 group",
                "Creating it under Teams & groups > Active teams & groups in the Microsoft 365 admin center provisions the mailbox, calendar, SharePoint site and Planner together. You choose a name, an email address, owners, members, and whether it's public or private. A group created with a team in Teams or a plan in Planner is the same kind of object underneath.",
                new Wrong("SharePoint admin center > Term store, then add a term set for the project team and assign its members to it", "The term store manages metadata and doesn't create groups."),
                new Wrong("Defender portal > Threat policies", "Threat policies configure security protection, not groups."),
                new Wrong("Exchange admin center > Mail flow > Rules", "Rules process messages and can't create a group.")),
            new Step(
                "What must you assign when creating the group?",
                "At least one owner, ideally two, who will manage its members",
                "Owners can add and remove members and change the group's settings, so the team runs itself without tickets to IT. With two owners, the group isn't left ownerless when one of them leaves the company. A group with no owner can only be managed by an admin.",
                new Wrong("A Global Administrator as the only member, so that membership changes stay with IT", "A group is meant to be run by its team, and this over-involves a privileged account."),
                new Wrong("A dedicated E5 license for the group", "A group doesn't need its own license."),
                new Wrong("A retention label on each member", "Retention labels apply to content, not to group membership.")),
            new Step(
                "Partners report that their emails to the group are being rejected. What should you change?",
                "Turn on 'Allow external senders to email this group' in the group's settings",
                "A Microsoft 365 Group rejects mail from senders outside the organization by default, so the partners get a bounce. The fix is the group setting 'Allow external senders to email this group', on the group's Settings tab in the admin center. In PowerShell it's Set-UnifiedGroup with -RequireSenderAuthenticationEnabled $false. Distribution lists have the same default and the same kind of switch.",
                new Wrong("Add each partner as a full member of the tenant", "That grants far more access than the requirement calls for."),
                new Wrong("Create a new inbound connector for the partners, so that the group accepts their domains", "Connectors handle routing with specific organizations. The rejection comes from the group's own setting."),
                new Wrong("Disable the spam filter for the group", "Spam filtering isn't the cause, and turning it off introduces new risk."))),

        new(
            "group-send-as",
            ScenarioCategory.GroupManagement,
            "Sending as the support distribution list",
            "Team lead Aisha needs to reply to customers as support@contoso.example, which is a distribution list. Customers must see only the support address, with no mention of Aisha.",
            "Send As makes a message appear to come directly from the group, while Send on Behalf shows 'Aisha on behalf of Support'. Exchange handles the two differently. Send As is a recipient permission granted through the group's delegates in the Exchange admin center or with Add-RecipientPermission. Send on Behalf is set with the GrantSendOnBehalfTo parameter, using Set-DistributionGroup for distribution lists or Set-UnifiedGroup for Microsoft 365 Groups. Permission changes aren't instant, so allow time for replication before testing. Aisha must also pick the address in the From field in Outlook.",
            new Step(
                "Which permission meets the requirement?",
                "Send As",
                "There are two ways to send for another address. With Send As, the message looks as if it came from support@contoso.example and the delegate isn't shown anywhere. With Send on Behalf, the From line reads 'Aisha on behalf of Support'. The customer must not see Aisha, so it has to be Send As. Full Access is a third permission, for opening a mailbox, and it doesn't include sending.",
                new Wrong("Send on Behalf", "Recipients would see 'Aisha on behalf of Support', which breaks the requirement."),
                new Wrong("Full Access", "That's a mailbox permission for reading a mailbox. It doesn't apply to a distribution list or let you send as it."),
                new Wrong("Group owner", "Owners manage membership but get no right to send as the group.")),
            new Step(
                "Where do you grant it in the admin UI (user interface)?",
                "Exchange admin center > Recipients > Groups > Distribution list > the group's Delegates settings",
                "In the Exchange admin center, open Recipients > Groups, pick the Distribution list tab, select the group, and edit Manage delegates in its settings. There you add Aisha and choose the Send As permission. The permission is stored on the group, which is why it's set there and not on Aisha's account.",
                new Wrong("Microsoft 365 admin center > Roles > Role assignments, then assign Aisha the Exchange Recipient Administrator role", "Admin roles grant administrative rights, not sending rights on a single group."),
                new Wrong("Microsoft Entra ID > App registrations", "App registrations are for applications and have no connection to mail delegation."),
                new Wrong("Outlook on the web > Aisha's signature settings", "A signature changes the message text, not the sender's permissions.")),
            new Step(
                "You'd rather script this. Which PowerShell command is right?",
                "Add-RecipientPermission -Identity support@contoso.example -Trustee aisha@contoso.example -AccessRights SendAs",
                "Add-RecipientPermission is the Exchange Online cmdlet for Send As. -Identity is the address being sent as, -Trustee is the person who gets the right, and -AccessRights SendAs is the right. Check it with Get-RecipientPermission -Identity support@contoso.example. Send on Behalf uses a different cmdlet and parameter, Set-DistributionGroup -GrantSendOnBehalfTo.",
                new Wrong("Add-MailboxPermission -Identity support@contoso.example -User aisha@contoso.example -AccessRights FullAccess -InheritanceType All", "This grants Full Access to a mailbox, and a distribution list doesn't have one."),
                new Wrong("Set-DistributionGroup -Identity support@contoso.example -GrantSendOnBehalfTo aisha@contoso.example", "This grants Send on Behalf, which reveals Aisha's name."),
                new Wrong("Add-DistributionGroupMember -Identity support@contoso.example -Member aisha@contoso.example", "Membership means Aisha receives the group's email, not that Aisha can send as it.")),
            new Step(
                "Aisha still can't send as the group right after you grant it. What's the most likely next step?",
                "Allow time for the permission to replicate, then have Aisha choose the support address in the From field",
                "Two things commonly go wrong straight after the grant. The permission can take up to an hour to apply across Exchange Online. And Outlook keeps sending from Aisha's own address until she shows the From field and chooses the support address. If Outlook's cached entry for the address bounces, typing it out in full or using Outlook on the web avoids it.",
                new Wrong("Grant the Exchange Administrator role to Aisha", "An admin role is unnecessary and wouldn't fix replication timing."),
                new Wrong("Recreate the distribution list", "This disrupts existing members and addresses without addressing the cause."),
                new Wrong("Convert the distribution list to a shared mailbox immediately", "A migration isn't justified just because a permission hasn't propagated yet."))),

        new(
            "spo-external-sharing-domain",
            ScenarioCategory.GroupManagement,
            "External sharing blocked for one partner",
            "The Project Atlas team keeps its files on a Microsoft 365 Group site. Owners can share with partners at adatum.example, but sharing with a new consultant at fabrikam.example fails with 'Your organization's policies don't allow you to share with these users.' Organization-level external sharing is set to New and existing guests.",
            "SharePoint and OneDrive sharing works in layers. The organization-level setting in the SharePoint admin center is the most permissive level allowed, and each site can match it or be more restrictive. Domain restrictions apply on top of both, either for the whole organization under Policies > Sharing ('Limit external sharing by domain') or for a single site. Sharing with a domain that isn't on an allow list fails with this policy message, which is why one partner works and another doesn't. SharePoint also uses Microsoft Entra B2B (business-to-business) for guests, so an allow or deny list in the Entra external collaboration settings can block the invitation too, and the two lists should agree. The narrow fix is adding the approved domain to the allow list, not loosening the sharing level or removing the restriction for everyone.",
            new Step(
                "The organization level already allows guests. Where do you look first?",
                "The Project Atlas site's sharing settings in the SharePoint admin center, then any domain limits",
                "SharePoint sharing is decided in layers. The organization setting is the most permissive level any site can have. Each site has its own level, which can be the same or stricter. Domain allow or block lists can then restrict which external domains are permitted, at either level. One partner works and the other doesn't, so the block depends on the domain, and the site's settings and the domain limits are where to look.",
                new Wrong("The consultant's guest account in Entra ID, to see whether sign-in is blocked", "The invitation failed before any guest account was created, so there's no account to check."),
                new Wrong("The guest access settings in the Teams admin center", "Teams guest access controls the guest experience in Teams, not sharing from SharePoint."),
                new Wrong("The data loss prevention policy matches in Microsoft Purview for the files the owners tried to share with the consultant", "A DLP (data loss prevention) block shows a policy tip about sensitive content, and it would affect adatum.example as well.")),
            new Step(
                "The site allows New and existing guests and has no domain list. Under Policies > Sharing, the organization limits sharing to allowed domains, and only adatum.example is listed. What's the cause?",
                "fabrikam.example isn't on the organization's allowed domains list",
                "'Limit external sharing by domain' has two modes. In allow mode, only the listed domains can be shared with, and every other domain is blocked. In block mode, only the listed domains are refused. This tenant uses allow mode with a single entry, adatum.example, so fabrikam.example is refused with the policy message the owners saw. The site wasn't the cause, since it has no list of its own and already allows guests.",
                new Wrong("The site's sharing level is more restrictive than the organization's", "You just confirmed the site allows New and existing guests."),
                new Wrong("The consultant's Microsoft account hasn't been verified yet", "Verification happens after an invitation is sent. This one was blocked before it was sent."),
                new Wrong("Fabrikam's own tenant has a cross-tenant access policy that blocks B2B (business-to-business) collaboration with outside organizations", "A block in the partner's tenant fails at redemption, not with your organization's sharing policy message.")),
            new Step(
                "Security approves Fabrikam as a partner. What do you change?",
                "Add fabrikam.example to the allowed domains, and to the Entra B2B allow list if one is used",
                "Add the approved domain to the allow list under Policies > Sharing in the SharePoint admin center. That admits Fabrikam and nobody else. Sharing with a new guest also creates a Microsoft Entra B2B guest account, and Entra has its own allow or deny list under External collaboration settings. If that list is in use and Fabrikam is missing, the invitation fails at that second gate, so update both.",
                new Wrong("Change the organization's sharing level to Anyone so links work without sign-in", "Anyone links remove identity checks for all content, and they still wouldn't satisfy the domain restriction."),
                new Wrong("Turn off 'Limit external sharing by domain' at the organization level so the consultant can be invited right away", "That opens sharing to every external domain to admit one partner."),
                new Wrong("Create an internal account for the consultant and assign it an E3 license", "That costs a license, bypasses guest governance, and gives a non-employee a full internal identity.")),
            new Step(
                "How do you confirm the fix?",
                "Have an owner share again, then confirm the consultant can redeem the invitation and open the file",
                "Test the whole path the consultant will use. An owner shares the file again, the consultant receives the invitation, signs in or enters a one-time passcode to redeem it, and opens the file. Each of those steps can fail separately, so seeing the domain in the list isn't enough. Allow a few minutes after the change before testing.",
                new Wrong("Check that fabrikam.example now appears in the allowed domains list", "That confirms the setting, not that the consultant can reach the file."),
                new Wrong("Share a test file with an adatum.example user", "adatum.example already worked, so this test can't prove the fix."),
                new Wrong("Search the unified audit log for SharingInvitationCreated events from the site's owners over the last week", "Those events come from before the change and say nothing about a new attempt."))),

        new(
            "teams-guest-blocked-by-label",
            ScenarioCategory.GroupManagement,
            "Team owner can't add a guest",
            "Mei owns the Vendor Onboarding team in Microsoft Teams. When Mei types partner@fabrikam.example to add a member, Teams shows 'We didn't find any matches' and never offers to add a guest. Guest access is On in the Teams admin center, and fabrikam.example is an approved sharing domain.",
            "Adding a guest to a team requires several settings to agree. Guest access must be on in the Teams admin center, the Microsoft Entra external collaboration settings must let the owner invite guests, and the Microsoft 365 Groups settings must allow owners to add people outside the organization. A sensitivity label applied to the team can also block guests for that one team, because container labels control the group's guest setting. If any layer says no, Teams doesn't offer 'Add as a guest'. Testing whether the problem affects every team or only one tells you right away whether to look at a tenant-wide setting or at the team's label. A label reflects a classification decision, so changing it needs the data owner's approval. After accepting the invitation, the guest also has to switch to the host organization in Teams before the team appears.",
            new Step(
                "What's the quickest way to narrow down the problem?",
                "Try adding the same guest to a different team to see whether the block is tenant-wide or only on this team",
                "Several settings must all allow a guest: Teams guest access, the Entra external collaboration settings, the Microsoft 365 Groups guest settings, and any sensitivity label on the team. The first three are tenant-wide, and a label affects one team. Trying the same guest in another team separates these in one test. If it fails everywhere, look at a tenant setting. If it fails only here, look at this team.",
                new Wrong("Ask the partner to create a Microsoft account first", "Guests can redeem with a work account, a Microsoft account or a one-time passcode, so the partner's account isn't what's blocking Mei."),
                new Wrong("Reinstall the Teams client on Mei's computer", "The search result reflects policy, not a client fault, and reinstalling removes nothing that blocks guests."),
                new Wrong("Turn guest access off and on again in the Teams admin center, then wait for the change to replicate across the tenant before trying again", "Toggling a setting that's already on changes nothing, and it disrupts existing guests.")),
            new Step(
                "Other teams can add the guest. Vendor Onboarding has the sensitivity label 'Confidential - Internal'. What do you check?",
                "Whether the label's container settings turn off external user access for the team",
                "A sensitivity label applied to a team is a container label. It doesn't encrypt anything by being there. It sets rules for the team itself, including privacy and whether external users can be added. If the label's external user access option is off, owners can't add guests to that team, even though the tenant allows guests. You check this on the label in the Microsoft Purview portal, in its groups and sites settings.",
                new Wrong("Whether the label applies encryption to the team's files", "Encryption affects who can open labeled files, not who can be added as a member."),
                new Wrong("Whether Mei has a Teams Premium license", "Adding guests to a team doesn't need Teams Premium."),
                new Wrong("Whether the Entra external collaboration settings restrict guest invitations to admin roles", "That setting is tenant-wide, but other teams can already add the guest.")),
            new Step(
                "The label blocks guests by design, but the business needs the partner in this team. What is the right fix?",
                "Get the data owner's approval for a label that allows guests, then relabel the team",
                "The label isn't a fault to fix. Someone classified this team as internal only. If the business now needs a partner in it, the data owner has to accept that and approve a label whose settings allow guests. Then you change the team's label, and adding the guest works. Removing the label or creating an internal account gets the same result by going around the control, and an interviewer will be listening for that difference.",
                new Wrong("Remove the sensitivity label from the team so the default settings apply", "This drops the label's other protections and bypasses the data owner's decision."),
                new Wrong("Create a member account for the partner in your tenant so the label's guest restriction doesn't apply", "This sidesteps the classification and gives an outside person an internal identity and license."),
                new Wrong("Assign Mei the Teams Administrator role", "Admin roles don't override a label's guest setting, and the role grants far too much access.")),
            new Step(
                "The guest accepted the invitation but says the team doesn't show up in Teams. What do you tell them?",
                "Switch to the Contoso organization in Teams, using the account switcher on the profile picture",
                "A guest account lives in your tenant, Contoso, and not in the guest's home tenant. Teams opens in the home organization by default and shows only its teams. The guest has to select their profile picture and switch to Contoso in the list of organizations. The Vendor Onboarding team appears there.",
                new Wrong("Wait 24 hours for the guest account to sync to Teams", "Guest access normally works within minutes. The missing step is switching organizations."),
                new Wrong("Resend the invitation from the Entra admin center", "The invitation was already redeemed, so another one changes nothing."),
                new Wrong("Sign in to Teams on the web in a new private browser window, which clears the cached tenant list from the last session", "A clean browser session still opens in the guest's home organization."))),

        // ---------------------------------------------------------------- Mail flow rules
        new(
            "mailflow-external-disclaimer",
            ScenarioCategory.MailFlowRules,
            "Append an external sender disclaimer",
            "After a phishing incident, Security wants a warning added to the bottom of every email that comes from outside the company, stating that the message originated externally.",
            "Mail flow rules (transport rules) have conditions, actions, and exceptions, and Exchange Online evaluates them as messages pass through the transport pipeline. The condition 'The sender is located > Outside the organization' targets inbound external mail. The action 'Apply a disclaimer to the message > append a disclaimer' adds the text to the bottom of the message, while 'prepend' places it at the top. The fallback option decides what happens if the disclaimer can't be inserted, such as for an encrypted message: Wrap creates a new message with the original attached, Ignore delivers it without the disclaimer, and Reject bounces it. Running the rule in test mode first lets you check what it matches without affecting live mail.",
            new Step(
                "Where do you build this?",
                "Exchange admin center > Mail flow > Rules > Add a rule",
                "Mail flow rules, also called transport rules, run on Exchange Online's servers while a message is in transit. That means they apply to every message for every user, whatever app or device is used, and users can't turn them off. Each rule has conditions, actions and optional exceptions. They are created in the Exchange admin center under Mail flow > Rules, or with New-TransportRule.",
                new Wrong("An Outlook inbox rule on each mailbox, deployed with a script", "Client rules are per mailbox, can be changed by users, and can't insert a disclaimer."),
                new Wrong("A Defender anti-phishing policy", "Anti-phishing policies apply protection and warnings, but they can't add custom disclaimer text."),
                new Wrong("A Microsoft Purview retention policy", "Retention policies govern how long content is kept, not its message text.")),
            new Step(
                "Which condition targets the right messages?",
                "The sender is located > Outside the organization",
                "A rule's condition decides which messages it touches. 'The sender is located > Outside the organization' matches mail from any sender who isn't in one of the tenant's accepted domains, which means inbound external mail. Getting the direction right matters. The recipient version of this condition would match your own outbound mail.",
                new Wrong("The recipient is located > Outside the organization", "That matches outbound mail, the opposite of what's needed."),
                new Wrong("The subject or body includes 'external'", "That depends on message content and misses nearly all external mail."),
                new Wrong("Apply to all messages", "Internal mail would be stamped as external too, and people would soon learn to ignore the warning.")),
            new Step(
                "Which action adds the warning to the bottom of each message, and what should happen if it can't be inserted?",
                "Apply a disclaimer > append a disclaimer, with the fallback action set to Wrap",
                "The action 'Apply a disclaimer to the message' has two forms. Append adds the text at the bottom, and prepend adds it at the top. Security asked for the bottom, so append. Some messages can't be changed, for example encrypted or signed ones, and the fallback action covers them. Wrap puts the original inside a new message that carries the disclaimer, Ignore delivers it with no disclaimer, and Reject bounces it. Wrap keeps the warning without losing mail.",
                new Wrong("Prepend the subject with 'EXTERNAL' and no disclaimer", "Changing the subject adds a tag, but it doesn't add the required warning text to the bottom of the message."),
                new Wrong("Apply a disclaimer > prepend a disclaimer, with the fallback action set to Reject", "Prepend puts the text at the top, and Reject would bounce legitimate encrypted mail."),
                new Wrong("Redirect the message to a Security mailbox", "That diverts mail away from the recipients instead of warning them.")),
            new Step(
                "The rule is saved. How do you roll it out safely?",
                "Run it in test mode, review the results, then switch to Enforce and enable the rule",
                "A new rule can run in three modes: Enforce, Test with Policy Tips, and Test without Policy Tips. In a test mode the rule's actions aren't applied, but matches are recorded, so message trace shows what it would have done. Once the matches look right, switch to Enforce and check that the rule is enabled. A wrong condition on an enforced rule changes every message at once.",
                new Wrong("Enforce the rule immediately with it enabled, then watch user reports through the first full day", "A mistake in the condition could affect every message before anyone notices."),
                new Wrong("Leave the rule in test mode permanently", "A rule in test mode never adds the disclaimer."),
                new Wrong("Restart the Exchange transport service", "Exchange Online is a cloud service, so admins can't restart its transport, and it wouldn't be needed anyway."))),

        new(
            "mailflow-rule-not-firing",
            ScenarioCategory.MailFlowRules,
            "Disclaimer rule skips some messages",
            "Your external disclaimer rule worked for weeks, but now emails from one vendor, vendor.example, arrive with no disclaimer. Other external mail is unaffected.",
            "Exchange Online evaluates mail flow rules in priority order, with 0 as the highest priority. If a matching rule has 'Stop processing more rules' turned on, later rules don't run for that message. This is a common cause of a rule that works for most mail but not for a specific sender. Message trace details list the rules that a message matched, so they show what actually happened, rather than what the rule list suggests should have happened. The safest fix is to move the disclaimer rule above the other rule, or to turn off the stop setting if it's no longer needed.",
            new Step(
                "How do you find out what happened to one of the vendor's messages?",
                "Run a message trace on a vendor message and open the details to see which rules it matched",
                "Don't reason from the rule list. Look at one real message. Run a message trace for a vendor message, open it, and read the events. The trace lists each mail flow rule that the message matched. If the disclaimer rule isn't in that list, the question becomes why it didn't run, and whichever rule did match is the first suspect.",
                new Wrong("Delete the rule and recreate it from scratch", "It works for other mail, so the rule itself isn't broken, and you'd lose the chance to find the cause."),
                new Wrong("Check the vendor's SPF (Sender Policy Framework) record", "SPF affects sender authentication. It wouldn't selectively skip a transport rule."),
                new Wrong("Check the affected users' Outlook view settings, in case the disclaimer is collapsed or hidden in the reading pane", "This happens to every recipient of that vendor's mail, so individual client settings aren't the cause.")),
            new Step(
                "The trace shows that a rule named 'Vendor Bypass' matched and your disclaimer rule didn't. What do you check?",
                "The priority order of the two rules, and whether 'Vendor Bypass' has 'Stop processing more rules' turned on",
                "Rules run in priority order, starting from 0. Each rule has an option called 'Stop processing more rules'. When a message matches a rule with that option on, no lower rule is evaluated for that message. 'Vendor Bypass' matches only vendor.example, so if it sits above the disclaimer rule and has the option on, it explains why exactly one sender loses the disclaimer.",
                new Wrong("Whether the vendor's domain has expired", "An expired domain wouldn't explain the mail arriving and matching a rule."),
                new Wrong("Whether the disclaimer text is over a length limit, or contains HTML (Hypertext Markup Language) that the rule can't insert into the vendor's messages", "That wouldn't produce a failure for just one sender."),
                new Wrong("The mailbox size of the recipients", "Mailbox size has no effect on which rules are evaluated.")),
            new Step(
                "That's the cause. What is the cleanest fix?",
                "Move the disclaimer rule above 'Vendor Bypass', or turn off 'Stop processing more rules' if it's not needed",
                "Two small changes fix this. Move the disclaimer rule to a higher priority than 'Vendor Bypass', so it runs first. Or clear 'Stop processing more rules' on 'Vendor Bypass', so later rules still run. Find out why the bypass rule exists before choosing, and leave whatever else it does alone.",
                new Wrong("Delete 'Vendor Bypass' without asking why it exists", "Someone created it for a reason, so removing it may break something else."),
                new Wrong("Add a second disclaimer rule at the bottom of the list", "It sits below the stop rule as well, so it would be skipped in the same way."),
                new Wrong("Add a disclaimer action to every other rule", "This duplicates logic and makes the rule set harder to maintain.")),
            new Step(
                "How do you confirm the change worked?",
                "Have a vendor message come through, then confirm the disclaimer and check the trace details",
                "Verify the same way you diagnosed it. Wait for the change to apply, get a real message from vendor.example, and check that it carries the disclaimer. Then open its message trace and confirm that the disclaimer rule now appears among the matched rules. A test from inside the company wouldn't match the external sender condition.",
                new Wrong("Trust that the new order is correct and close the ticket", "This skips verification, and rule changes may take time to take effect."),
                new Wrong("Send an internal test message", "An internal message doesn't meet the external sender condition, so the rule wouldn't match."),
                new Wrong("Check the Exchange admin center service health page only", "Service health reports Microsoft outages, not whether your rule fired."))),

        new(
            "defender-phish-campaign",
            ScenarioCategory.MailFlowRules,
            "Responding to a credential phishing campaign",
            "At 10:40, three users report an email titled 'Payroll update required' that links to a fake Microsoft sign-in page. It came from hr-notice@payroll-portal.example and passed SPF (Sender Policy Framework) and DKIM (DomainKeys Identified Mail) for that domain. The tenant has Microsoft Defender for Office 365 Plan 2.",
            "Threat Explorer in Defender for Office 365 Plan 2 finds every copy of a campaign by sender, subject or URL (uniform resource locator), and shows each copy's delivery location along with URL clicks recorded by Safe Links. You can act on the results directly, for example with a soft delete, and the Action center tracks the remediation. Zero-hour auto purge (ZAP) can also remove delivered messages when a verdict changes, but you can't rely on its timing during an active campaign. A sender that passes SPF (Sender Policy Framework) and DKIM (DomainKeys Identified Mail) for its own lookalike domain isn't spoofing you, so your own DMARC (Domain-based Message Authentication, Reporting and Conformance) policy won't stop it, and the Tenant Allow/Block List is the place to block the domain and the URL. Anyone who clicked through to the credential page may have given away a password, so reset it, revoke their sessions and review their sign-ins.",
            new Step(
                "How do you find everyone who received the message?",
                "Search Threat Explorer in the Defender portal by sender address and subject for today",
                "Threat Explorer, under Email & collaboration > Explorer in the Defender portal, searches all mail in the tenant. Filter on the sender address, the subject or the URL (uniform resource locator), and it returns every copy with its recipient, where it was delivered (Inbox, Junk or quarantine), and whether anyone clicked the link. It comes with Defender for Office 365 Plan 2. Three reports usually mean many more recipients, and this is how you find the full scope.",
                new Wrong("Ask the three reporters to forward the message to the service desk", "Their copies don't tell you who else received it."),
                new Wrong("Run a mail flow rule report for messages from payroll-portal.example", "Rule reports show which rules matched, not a full list of recipients."),
                new Wrong("Check the Service health page in the Microsoft 365 admin center for an active phishing advisory affecting the tenant", "Service health reports Microsoft service issues, not campaigns aimed at your users.")),
            new Step(
                "Explorer shows 140 recipients, with 128 copies still in Inboxes. How do you remove them?",
                "Select the messages in Explorer, take a Soft delete action, and track it in the Action center",
                "From the Explorer results you select the messages and choose Take action. A soft delete moves each copy to the Recoverable Items folder, where users can't see it but an admin can still recover it. The Action center records which mailboxes were cleaned and which failed. It's faster and more complete than asking 140 people to delete a message.",
                new Wrong("Create a mail flow rule that deletes messages from payroll-portal.example", "Mail flow rules act on messages in transit, so they can't touch copies that were already delivered."),
                new Wrong("Wait for zero-hour auto purge to remove them", "ZAP (zero-hour auto purge) only acts when a verdict changes, and you can't count on its timing while users are clicking."),
                new Wrong("Email all staff a warning, then ask each recipient to delete the message and empty Deleted Items", "People miss warnings, and some will click before they read it.")),
            new Step(
                "How do you block further messages and the link itself?",
                "Add block entries for the domain and URL in the Tenant Allow/Block List",
                "The Tenant Allow/Block List, under Threat policies in the Defender portal, takes block entries for domains and addresses, URLs, and files. A domain block stops further mail from payroll-portal.example. A URL block makes Safe Links stop the click, which also covers a copy sent from a different address. The sender passed SPF and DKIM for a domain the attacker owns, so authentication checks won't catch it.",
                new Wrong("Publish a DMARC (Domain-based Message Authentication, Reporting and Conformance) policy of p=reject for your own domain", "The attacker sends from its own domain and passes its own SPF and DKIM. Your DMARC policy only protects your domain."),
                new Wrong("Block the sending IP (Internet Protocol) address in the connection filter policy", "Campaigns rotate sending IPs, and an IP block does nothing about the URL in other messages."),
                new Wrong("Add payroll-portal.example to the protected domains for impersonation in the anti-phishing policy", "Impersonation protection guards domains you own or care about. The attacker's domain isn't impersonating anything.")),
            new Step(
                "Explorer's URL clicks show that two users clicked through the Safe Links warning. What do you do next?",
                "Treat both accounts as compromised: reset passwords, revoke sessions, and review sign-ins",
                "Clicking past the warning means the user reached the fake sign-in page and may have typed a password. Assume they did. Reset the password, revoke sessions so stolen tokens stop working, and review the sign-in logs for sign-ins from unfamiliar locations. Also check for new inbox rules, forwarding and newly registered MFA (multi-factor authentication) methods, which attackers add to keep access.",
                new Wrong("Run a full antivirus scan on both users' computers", "Credential phishing steals passwords in the browser, and a malware scan doesn't undo that."),
                new Wrong("Assign both users a phishing awareness training module", "Training is worthwhile later, but it doesn't contain a possible compromise now."),
                new Wrong("Move both users into the Strict preset security policy so future phishing messages are quarantined before they reach the Inbox", "That protects against the next message, not against credentials that may already be stolen."))),

        // ------------------------------------------------------------------- VOIP support
        new(
            "voip-one-way-audio",
            ScenarioCategory.VoipSupport,
            "One-way audio at a branch office",
            "Every desk phone at a branch office can place and answer calls through the company's hosted PBX (private branch exchange), but on all calls the staff hear nothing while the people they call hear them clearly. Phones show as registered, and the problem began after the branch firewall was replaced.",
            "SIP (Session Initiation Protocol) sets up a call, and RTP (Real-time Transport Protocol) carries the audio as a separate UDP (User Datagram Protocol) stream, so a call can connect perfectly with audio failing in one direction. Each side uses the SDP (Session Description Protocol) body in the SIP messages to announce the IP (Internet Protocol) address and port where it wants to receive RTP. When a phone behind NAT (Network Address Translation) advertises its private address, the far end sends audio to an address it can't route to. This is the classic one-way audio pattern, and it fits here because the new firewall changed how NAT was handled. Fix it with proper NAT traversal, such as STUN (Session Traversal Utilities for NAT), or an SBC (session border controller) or PBX (private branch exchange) that uses symmetric RTP. Also disable SIP ALG (Application Layer Gateway) if the firewall's rewriting is making things worse, and make sure the RTP UDP port range is allowed. Compare captures from both sides of the firewall, because they show whether a stream was never sent or was lost on the way.",
            new Step(
                "Calls connect normally, but audio only flows one way. What does that tell you?",
                "SIP (Session Initiation Protocol) signaling is working, so the fault is in the RTP (Real-time Transport Protocol) media path",
                "A VoIP (Voice over IP) call uses two protocols. SIP is the signaling that makes the phone ring and connects the call. RTP carries the audio, as a separate UDP (User Datagram Protocol) stream in each direction, on different ports. These calls ring and connect, so SIP works. One direction of audio is missing, so one RTP stream isn't arriving. That narrows the fault to the media path, and the new firewall is the obvious suspect.",
                new Wrong("The phones and the hosted PBX have a codec mismatch, so audio can't be decoded on one side", "With no common codec the call normally fails during setup, often with 488 Not Acceptable Here. It wouldn't connect and carry one good audio direction."),
                new Wrong("SIP (Session Initiation Protocol) registration is failing intermittently at the branch", "The phones show as registered and can make and receive calls, so registration isn't the problem."),
                new Wrong("The handsets' earpieces need to be replaced at the branch", "Every phone at the site failing at once, right after a firewall change, points to the network and not to hardware.")),
            new Step(
                "How do you get evidence of what is happening to the audio?",
                "Take a packet capture during a test call and inspect the SIP and RTP streams in Wireshark",
                "A packet capture shows what's on the wire. In Wireshark, Telephony > VoIP Calls lists each call, and Flow Sequence draws the SIP messages. Inside the INVITE and the 200 OK is the SDP (Session Description Protocol), where each side states the IP (Internet Protocol) address and port it wants audio sent to. Telephony > RTP > RTP Streams shows which audio streams exist. With those you can see whether a stream was never sent or was sent to the wrong place.",
                new Wrong("Run a sustained ping from a phone's VLAN (virtual local area network) to the hosted PBX address", "Ping proves basic reachability with ICMP (Internet Control Message Protocol). It says nothing about which addresses and ports RTP is using."),
                new Wrong("Review the call detail records in the hosted PBX portal to see which direction of each call failed", "CDRs (call detail records) show that the call connected and how long it lasted. They don't show which way the media was flowing."),
                new Wrong("Run an internet speed test from a PC on the branch network", "Bandwidth shortage causes poor quality on both directions, not a clean loss of audio in one.")),
            new Step(
                "In the capture, the phone's SDP says c=IN IP4 192.168.10.25. RTP from the phone leaves the site, but no RTP comes back. What is the cause?",
                "The phone is advertising a private IP, so the far end sends RTP to an unroutable address",
                "In SDP, the c= line is the address where that side wants to receive audio. The phone put its private LAN (local area network) address there, 192.168.10.25. The provider sends its RTP to that address, and private addresses can't be routed on the internet, so those packets go nowhere. Audio from the phone still gets out, because outbound packets pass through NAT (Network Address Translation) normally. That's why the staff hear nothing and the other party hears them.",
                new Wrong("DSCP (Differentiated Services Code Point) markings are missing from the voice traffic, so the ISP (internet service provider) is dropping the return RTP packets", "Missing QoS (quality of service) markings lower priority under congestion. They don't remove one direction of audio completely."),
                new Wrong("The jitter buffer on the phones is too small for this WAN (wide area network) link", "A small jitter buffer causes choppy audio from late packets. Here no return packets arrive at all."),
                new Wrong("The SRTP (Secure Real-time Transport Protocol) keys don't match, so the phone is discarding the audio", "A key mismatch would show RTP packets arriving but undecodable. This capture shows no return stream at all.")),
            new Step(
                "What is the right fix?",
                "Configure NAT traversal so SDP carries a reachable address, and allow the RTP port range",
                "The far end needs a public address and port to send audio to. STUN (Session Traversal Utilities for NAT) lets the phone learn its public address and put it in the SDP. Or the provider's SBC (session border controller) can use symmetric RTP, where it ignores the SDP address and replies to wherever the phone's audio came from. The firewall must also allow the provider's RTP UDP port range. If SIP ALG (Application Layer Gateway) on the new firewall is rewriting packets badly, turn it off.",
                new Wrong("Open TCP (Transmission Control Protocol) port 5060 inbound from the hosted PBX to the phones so their audio can reach the branch", "Port 5060 carries SIP signaling, which already works. RTP uses a range of UDP ports."),
                new Wrong("Force every phone to use the G.711 codec instead of G.729", "Changing the codec changes how audio is encoded, not the unreachable address in the SDP."),
                new Wrong("Shorten the SIP registration interval on all branch phones", "Registration keeps signaling reachable. It doesn't change where the far end sends RTP.")),
            new Step(
                "How do you confirm the fix?",
                "Test calls in both directions, confirming two-way audio and two RTP streams in a capture",
                "Test what was broken, in both directions. Make an outbound call and an inbound call through the PBX, and check that both people can hear each other. Then confirm in a capture that there are two RTP streams, one each way. Registration and internal calls worked during the fault, so they can't prove the fix.",
                new Wrong("Confirm that all of the phones still show as registered", "They were registered during the fault too, so this proves nothing about audio."),
                new Wrong("Check that the firewall rule's hit counter is going up on the new RTP rule during several live calls", "A rising counter shows traffic matched the rule, not that a person can hear audio in each direction."),
                new Wrong("Place a single internal extension-to-extension test call", "Internal calls may stay on the local network and never cross the NAT path that was failing."))),

        new(
            "voip-sip-408-timeout",
            ScenarioCategory.VoipSupport,
            "SIP 408 Request Timeout on a remote phone",
            "Calls to a remote worker's extension fail after about 32 seconds, and the caller's phone shows 408 Request Timeout. The phone sits behind a home router and shows as registered. Oddly, it's reachable for a minute or two right after it's rebooted, and calls made from that phone always work.",
            "A SIP (Session Initiation Protocol) 408 means a request got no final response within the allowed time. For an INVITE over UDP (User Datagram Protocol), Timer B expires at 64 times T1, which is 32 seconds with the default 500 ms T1, and that matches this ticket. Retransmissions with no provisional response show the INVITE isn't reaching the phone, or the phone's reply isn't getting back. Outbound calls work because the phone opens its own path through the router. Inbound calls depend on a NAT (Network Address Translation) or firewall UDP mapping staying open. When the mapping expires between registrations, the PBX (private branch exchange)'s registration is still valid, but its INVITE is dropped at the router. Keep-alives sent more often than the router's UDP timeout hold the mapping open. Avoid changing SIP timers, because a longer wait just delays the same failure.",
            new Step(
                "What does SIP (Session Initiation Protocol) 408 Request Timeout tell you?",
                "No final response to the request arrived in time, so find where the request stopped",
                "SIP response codes follow the same pattern as HTTP (Hypertext Transfer Protocol). 4xx is a request failure, and 408 Request Timeout means a request was sent and no final response came back in time. It doesn't say why. A 408 is generated by whichever device gave up waiting, so the work is to find how far the INVITE got.",
                new Wrong("The called user pressed decline on the handset", "A deliberate decline usually returns 603 Decline, or 486 Busy Here, not a timeout."),
                new Wrong("The PBX (private branch exchange) searched its dial plan, found no such extension, and rejected the call with a timeout", "An unknown destination returns 404 Not Found, usually straight away rather than after 32 seconds."),
                new Wrong("The caller failed digest authentication on the PBX (private branch exchange)", "Authentication challenges return 401 or 407, and they arrive immediately.")),
            new Step(
                "How do you see where the call is failing?",
                "Capture the call's SIP signaling on the PBX with sngrep or tcpdump and read the ladder",
                "sngrep runs on the PBX and draws each call as a ladder diagram, which shows every SIP message between the PBX and the phone in order, with timing. tcpdump can save the same traffic for Wireshark. The ladder shows whether the PBX sent the INVITE, where it sent it, and whether anything came back.",
                new Wrong("Reboot the PBX during a quiet period and retest the call", "This destroys the evidence, affects every user, and doesn't explain a single phone's failure."),
                new Wrong("Check the RTP (Real-time Transport Protocol) packet statistics for the failed call on the PBX to see where audio stopped flowing", "No media is set up for a call that never connected, so there are no RTP statistics to read."),
                new Wrong("Replace the remote worker's handset with a spare phone", "Outbound calls work, so the phone isn't obviously broken, and swapping it skips the diagnosis.")),
            new Step(
                "The ladder shows the PBX resending the INVITE to the phone's registered public address at 0.5, 1, 2 and 4 seconds, with no 100 Trying or 180 Ringing. What does this mean?",
                "The INVITE isn't reaching the phone, or its replies aren't returning, until Timer B expires",
                "Over UDP (User Datagram Protocol), SIP resends a request that gets no response. Timer A starts at T1, which is 500 ms by default, and doubles each time, which gives the gaps of 0.5, 1, 2 and 4 seconds. A phone that receives an INVITE answers almost at once with 100 Trying, then 180 Ringing. Neither appears, so the INVITE never arrived, or the answers were lost. After 32 seconds, which is Timer B, the PBX gives up and returns 408 to the caller.",
                new Wrong("The phone is ringing, but nobody is there to answer it", "A ringing phone sends 180 Ringing, and that never appears in this ladder."),
                new Wrong("The PBX is overloaded and is shedding incoming calls, so it's resending each INVITE until one gets through", "An overloaded server normally answers 503 Service Unavailable, and other extensions work fine."),
                new Wrong("The two ends have no codec in common for this call", "A codec failure comes back as a response such as 488. Here there's no response of any kind.")),
            new Step(
                "The phone last registered 40 minutes ago, and the home router's UDP timeout is 60 seconds. What is the best fix?",
                "Enable SIP keep-alives, sent more often than the router's 60-second UDP timeout",
                "When the phone registers, the home router creates a NAT (Network Address Translation) mapping from the phone to the PBX, and the PBX sends calls back through that mapping. The router deletes an idle UDP mapping after 60 seconds. The registration stays valid for much longer, so the PBX keeps sending INVITEs to a mapping that no longer exists, and the router drops them. A keep-alive every 20 to 30 seconds keeps the mapping open. This explains the other clues too. A reboot re-registers and opens a new mapping for a minute, and outbound calls open their own.",
                new Wrong("Raise Timer B on the PBX from 32 seconds to 120 seconds so the phone has more time to respond", "The INVITE is being dropped at the router, so waiting longer just makes the same failure slower."),
                new Wrong("Move the extension to a different number on the PBX", "The number isn't the problem. The new extension would hit the same expired mapping."),
                new Wrong("Port-forward all UDP ports on the home router to the phone", "This exposes the phone to the internet and is hard to maintain, when a keep-alive solves the actual cause.")),
            new Step(
                "How do you prove the fix works?",
                "Wait well past the 60-second UDP timeout, then call in and confirm 100 Trying and 180 Ringing",
                "Reproduce the conditions of the failure. The fault needs the NAT mapping to expire, so leave the phone idle for several minutes, well past 60 seconds, then call it. The ladder should now show the INVITE answered by 100 Trying and 180 Ringing, and the phone should ring. A test straight after a reboot would have passed before the fix.",
                new Wrong("Call the phone straight after you've rebooted it", "This phone already worked right after a reboot, so that test would pass even without a fix."),
                new Wrong("Have the remote worker place an outbound test call", "Outbound calls always worked. The problem is only with inbound calls."),
                new Wrong("Confirm that the phone is displaying a green registered icon", "It showed as registered during the failures too, so the icon proves nothing."))),

        // -------------------------------------------------------- Network troubleshooting
        new(
            "network-packet-loss-mtr",
            ScenarioCategory.NetworkTroubleshooting,
            "Diagnosing packet loss with ping and MTR",
            "Users at a branch office report choppy video calls and stalls in a cloud app at 203.0.113.50, starting this morning. Everything on the local file server works normally. You're on a Linux machine on the branch LAN (local area network).",
            "Ping is the quickest way to scope a problem: test the default gateway first, then an external target. MTR (My Traceroute) then combines ping and traceroute by sending a stream of probes to every hop. The rule for reading it is that loss only matters if it continues through to the final destination. A router in the middle showing loss when the hops after it don't is usually just rate limiting the ICMP (Internet Control Message Protocol) or TTL-expired replies it generates, and it's forwarding traffic fine. Real loss starts at one hop and carries through to the end, which points to that hop or the link leading to it. Paths are often asymmetric, so run MTR from the far side too if you can, and check interface errors and utilization on your own equipment before escalating. Give the ISP (internet service provider) timestamped MTR reports from both directions.",
            new Step(
                "Where do you start?",
                "Ping the default gateway, then an external IP (Internet Protocol), each with about 100 packets",
                "Start by finding out which side of the gateway the loss is on. Ping the default gateway to test the LAN, then an external address to test the path beyond it. Use around 100 packets, for example ping -c 100, because the result is a percentage and a few packets can easily miss occasional loss. A clean gateway with loss outside points to the WAN (wide area network).",
                new Wrong("Ping 203.0.113.50 with the default four packets, since that's the address users are complaining about", "Four packets can't measure intermittent loss, and one target doesn't tell you where the loss is."),
                new Wrong("Run nslookup for the cloud app's hostname from a client", "Name resolution isn't what's failing. Users connect, but the connection is unstable."),
                new Wrong("Restart the branch core switch to clear out its counters", "This causes an outage, wipes evidence, and the local file server is already working fine.")),
            new Step(
                "The gateway shows 0% loss, and the external IP shows 5%. Which command shows where the loss begins?",
                "mtr -rwc 200 203.0.113.50",
                "MTR (My Traceroute) combines traceroute and ping. It finds every router on the path and then probes each one many times, reporting loss and latency per hop. In this command, -r is report mode, -w keeps full hostnames, and -c 200 sends 200 probes to each hop. With 200 probes, a 5% loss shows up reliably.",
                new Wrong("traceroute -n -m 30 203.0.113.50", "Traceroute sends only three probes per hop, which is too few to measure intermittent loss."),
                new Wrong("arp -a", "The ARP (Address Resolution Protocol) table shows local IP-to-MAC mappings and tells you nothing beyond the LAN."),
                new Wrong("netstat -an", "This lists local sockets and their state, not the loss along a network path.")),
            new Step(
                "MTR shows hop 5 at 40% loss, but hops 6 to 9, including the destination, show 0%. What do you conclude?",
                "Ignore hop 5, since loss that doesn't carry on is likely ICMP (Internet Control Message Protocol) rate limiting",
                "Each line of MTR is that router's reply to probes addressed to it, and routers generate those ICMP replies in their control plane, at low priority and often rate limited. Traffic passing through a router is forwarded by the data plane, which is separate and much faster. If hop 5 were dropping forwarded packets, hops 6 to 9 would show loss as well, because their probes pass through it. They show 0%, so hop 5 is forwarding fine. The rule is that loss only counts when it continues to the final hop.",
                new Wrong("Hop 5 is dropping real traffic, so open a priority ticket with the network that owns that router", "Later hops prove traffic is passing through hop 5 cleanly, so that ticket would be rejected."),
                new Wrong("Add every hop's figure together to get a 40% end-to-end loss", "Loss isn't additive across hops, and the destination itself shows 0%."),
                new Wrong("The destination is hiding its true loss by blocking ICMP (Internet Control Message Protocol)", "A host blocking ICMP would show 100% loss. It wouldn't show a clean 0%.")),
            new Step(
                "A later MTR shows 0% at hops 1 and 2, then 6% at hop 3, and about 6% on every hop through to the destination. Hop 2 is your firewall, and hop 3 is the ISP (internet service provider)'s gateway. What does this show?",
                "Real loss begins at hop 3 or the link into it, which is your ISP handoff",
                "This is the pattern of real loss. It begins at one hop, and every hop after it shows about the same figure, right through to the destination. The first hop with loss is hop 3, so the fault is at hop 3 or on the link between hop 2 and hop 3. Hop 2 is your firewall and hop 3 is the ISP's gateway, so that link is the handoff to the ISP.",
                new Wrong("The destination server is overloaded and dropping requests", "The server is only where the loss ends up. It clearly starts several hops earlier."),
                new Wrong("The final router before the destination is at fault because it's the last hop to show any loss", "Each later hop just inherits loss from upstream, so read from where it starts."),
                new Wrong("The LAN switch is dropping frames between clients and the firewall", "Hops 1 and 2 show 0%, so traffic is getting through the LAN and the firewall cleanly.")),
            new Step(
                "What should you do before contacting the ISP?",
                "Check the WAN interface for errors and saturation, then run MTR from the far side too",
                "The suspect link has two ends, and one of them is yours. Check the firewall's WAN interface counters for CRC (cyclic redundancy check) errors, a speed or duplex mismatch, and utilization near the limit, since any of those would make it your fault to fix. Traffic often returns by a different route, and MTR only shows the outbound path, so a second MTR run from the far side toward you covers the return path.",
                new Wrong("Disable ICMP on the firewall so the MTR reports look cleaner", "That hides evidence and wouldn't change any real packet loss."),
                new Wrong("Raise the WAN interface MTU (maximum transmission unit) to 9000 to improve throughput", "Jumbo frames generally don't work across an ISP link, and this could add new faults."),
                new Wrong("Have users switch to a mobile hotspot until the loss clears up", "A workaround may help a single user, but it doesn't diagnose anything or restore the site.")),
            new Step(
                "The firewall's interface is clean and under 40% utilized. What is the right escalation?",
                "Open an ISP ticket with timestamped MTR reports from both directions",
                "An ISP acts fastest on evidence. Send MTR reports with timestamps from both directions, which show loss starting at its gateway, along with the clean interface counters that rule out your equipment. That lets the engineer go straight to the handoff. A vague report of slow internet starts with the first-line script.",
                new Wrong("Tell the ISP the internet is slow and request a callback", "With no evidence, the ISP is likely to start from the beginning or close the ticket."),
                new Wrong("Replace the firewall, since it's the last device you control before traffic reaches the ISP", "Its interface is clean, so this is expensive and doesn't address the evidence."),
                new Wrong("Open a ticket with the cloud app vendor about its server", "The loss begins well before the vendor's network, so the vendor can't fix it."))),

        new(
            "network-dhcp-scope-exhaustion",
            ScenarioCategory.NetworkTroubleshooting,
            "Resolving DHCP scope exhaustion",
            "At 9 AM, new devices on the staff and guest VLAN (virtual local area network) 20 (192.168.20.0/24) can't get online and show 169.254.x.x addresses. Devices that were already connected still work. The VLAN uses a Windows DHCP (Dynamic Host Configuration Protocol) server, and a conference has been running at the office all week.",
            "A 169.254.x.x address is an APIPA (Automatic Private IP Addressing) link-local address, which a client assigns itself when no DHCP (Dynamic Host Configuration Protocol) server offers a lease. Devices that already hold a lease keep working, which is the classic sign of exhaustion rather than a server outage. A /24 has only 254 usable addresses, and a Windows DHCP scope defaults to an 8-day lease, so visitors' phones keep addresses long after leaving. Phones using private (randomized) MAC (media access control) addresses can make this worse, since one device can look like several clients. Shorten the lease on busy guest networks, and reclaim stale leases. Changing the duration only affects new leases and renewals, so it needs some time or lease cleanup. Turn on conflict detection while reclaiming, since a device might still be using a removed address. Then fix capacity properly by widening the subnet or moving guests to their own VLAN (virtual local area network).",
            new Step(
                "What does a 169.254.x.x address tell you?",
                "The client got no DHCP offer and assigned itself a link-local APIPA (Automatic Private IP Addressing) address",
                "A DHCP lease takes four messages: Discover, Offer, Request and Acknowledge. The client broadcasts a Discover and waits for a server's Offer. If no offer comes, Windows and macOS give themselves an address in 169.254.0.0/16, called APIPA or link-local. It only works on the local segment and has no gateway. So this address always means the client asked for a lease and got no answer.",
                new Wrong("A DNS (Domain Name System) server has handed out an address from the wrong zone", "DNS resolves names and has no part in assigning a client's IP (Internet Protocol) address."),
                new Wrong("The client was given a reserved address by the DHCP server because its MAC (media access control) is in the exclusion list", "A DHCP reservation would give an address from the scope, not a 169.254.x.x address."),
                new Wrong("The default gateway is performing NAT (Network Address Translation) for the guest VLAN", "NAT doesn't assign addresses to clients, and 169.254.x.x isn't a NAT range.")),
            new Step(
                "Existing devices work, but new ones don't. What do you check first?",
                "The VLAN 20 scope statistics on the DHCP server, with Get-DhcpServerv4ScopeStatistics",
                "Two things can leave a client without an offer. Either the server can't be reached, or it has nothing to give. Clients that already hold a lease are renewing successfully, so the server is reachable. Get-DhcpServerv4ScopeStatistics, or the scope's statistics in the DHCP console, shows addresses in use, addresses free and the percentage used, which confirms an empty pool in one command.",
                new Wrong("Run ipconfig /renew on each affected device in turn", "Renewing asks for a lease again, and it fails the same way if the pool has none left."),
                new Wrong("Restart the DHCP Server service with Restart-Service DHCPServer to clear out the address pool", "Leases persist across a restart, so this interrupts service and frees nothing."),
                new Wrong("Replace the wireless access points serving the floor", "Wired and wireless clients on the VLAN share the same symptom, and existing clients work.")),
            new Step(
                "The scope shows 100% in use. The lease list is full of phones from the conference, with 8-day leases and many private MAC addresses. What is the root cause?",
                "Short-stay devices hold 8-day leases, exhausting the 254 addresses of the /24 scope",
                "A lease reserves an address for its whole duration, even after the device has left the building. A /24 gives 254 usable addresses, and the Windows default lease is 8 days. During a week-long conference, each visitor's phone holds an address for 8 days after one visit. A phone that uses a private Wi-Fi address can appear as a new client on a later day and take another one. The demand is real, and the leases are held far too long for it.",
                new Wrong("A rogue DHCP server on the VLAN is handing out bad addresses and taking leases from the real server", "Rogue DHCP gives clients a wrong address. It doesn't leave them with no offer at all."),
                new Wrong("The DHCP server's database has become corrupted", "The lease list is readable and accurate, and it shows real demand."),
                new Wrong("The gateway's ARP (Address Resolution Protocol) cache is full and is blocking new devices", "ARP is separate from DHCP, and the scope statistics already explain the failure.")),
            new Step(
                "What is the best immediate fix?",
                "Shorten the lease to a few hours, reclaim stale leases, and turn on conflict detection",
                "The quick fix has three parts. Shorten the lease on this scope to a few hours, so addresses return soon after a visitor leaves. That only takes effect when a client renews or gets a new lease. Delete the stale leases to free addresses now. Turn on conflict detection, which makes the server ping an address before offering it, in case a device is still using one you deleted.",
                new Wrong("Extend the lease duration to 30 days to stop clients renewing", "Longer leases hold addresses for even longer, which makes exhaustion worse."),
                new Wrong("Delete the scope and recreate it from scratch during the day so that every lease is released at once", "This discards the lease records and risks duplicate addresses with clients still using them."),
                new Wrong("Assign static IP (Internet Protocol) addresses to each device that can't connect", "Static addresses don't scale for visitors, and they would collide with addresses the scope hands out.")),
            new Step(
                "Service is back. How do you prevent it from happening again?",
                "Move guests to a separate VLAN, or widen the subnet to a /23 during a change window",
                "The lease change manages the pool better, but it doesn't make it bigger. Widening to a /23 doubles the subnet to 510 usable addresses, and the mask must change on the gateway, the scope and any static devices together. A separate guest VLAN with its own scope gives visitors their own pool and also keeps them off the staff network, which is the better design.",
                new Wrong("Add a second scope in another subnet with no gateway address for it", "With no router interface in that subnet, those clients couldn't reach anything."),
                new Wrong("Set a scope threshold alert and make no other changes", "Alerts are worth adding, but they only warn you. They don't add a single address."),
                new Wrong("Turn off the private MAC address feature on visitors' phones", "You don't control visitors' devices, and the pool would still be too small."))),

        // ------------------------------------------------------------- DNS troubleshooting
        new(
            "dns-external-forwarders",
            ScenarioCategory.DnsTroubleshooting,
            "External names fail while internal names resolve",
            "At 8 AM the whole office reports that 'the internet is down'. Websites won't load, but the intranet and file shares work. The PCs use the internal Windows DNS (Domain Name System) server at 10.0.0.10, and the office changed ISP (internet service provider) last night.",
            "A name failure looks like an outage to users, so separate connectivity from resolution first. A ping to a public IP (Internet Protocol) address that replies proves the path is up, and nslookup then tests DNS (Domain Name System) by itself. nslookup prints the server it asked, and a second argument makes it ask a different one, as in nslookup www.microsoft.com 1.1.1.1, so one command compares your server with a public resolver. An internal DNS server answers for its own zones from its own data. For every other name it either forwards the query to the resolvers on its Forwarders list or works down from the root servers using root hints. If the forwarders stop answering and root hints are turned off, internal names keep working and everything external fails. ISP (internet service provider) resolvers usually answer only that ISP's customers, so they are a common casualty of a provider change. Fix the Forwarders list on the server with Set-DnsServerForwarder, clear its cache with Clear-DnsServerCache, and test. Don't point clients at a public resolver to get round it, because it can't answer for internal zones and domain sign-in depends on those.",
            new Step(
                "On an affected PC, ping 8.8.8.8 gets replies, but www.microsoft.com won't open in a browser. What do you test next?",
                "Run nslookup www.microsoft.com to test name resolution on its own",
                "Replies from 8.8.8.8 show that the PC, the gateway and the new ISP link are passing traffic. A browser needs one more thing before it can connect, which is an address for the name. nslookup asks the DNS server for that address and nothing else, and it prints which server it asked. If it fails while a ping to an IP address works, the fault is name resolution and not connectivity.",
                new Wrong("Restart the firewall, since the ISP (internet service provider) change was made on it last night and nothing has loaded since", "Traffic already reaches 8.8.8.8 through the firewall, so a restart interrupts everyone and tests nothing."),
                new Wrong("Run tracert 8.8.8.8", "The ping already proved that path works. Another test of the same path adds nothing."),
                new Wrong("Reset the browser's settings and clear its cache on the PC", "Every PC in the office fails the same way, so one browser's settings aren't the cause.")),
            new Step(
                "nslookup shows Server: 10.0.0.10 and times out for www.microsoft.com. The same command resolves intranet.corp.example at once. What does that tell you?",
                "PCs can reach the DNS server, but it can't resolve names outside its own zones",
                "The intranet answer proves three things: the PC is pointed at 10.0.0.10, the server is reachable, and the DNS service is running. The server holds the corp.example zone itself, so it answers those names from its own data. It holds nothing for microsoft.com, so it has to ask other servers on the internet, and that is the part that's failing.",
                new Wrong("The DNS Server service on 10.0.0.10 has stopped", "A stopped service couldn't have answered for intranet.corp.example."),
                new Wrong("Every PC has a corrupted local DNS cache, and each one needs ipconfig /flushdns before external names will resolve again", "nslookup doesn't read the PC's cache. It asks the server directly, and the server is what timed out."),
                new Wrong("The microsoft.com zone is down", "Every external name fails, not one, and a fault that broad points to your own resolver.")),
            new Step(
                "How do you prove that the fault is in your DNS server's path to the internet, and not in the internet link itself?",
                "Run nslookup www.microsoft.com 1.1.1.1 to ask a public resolver directly",
                "A second argument tells nslookup which server to ask, so this sends the same question to a public resolver and bypasses 10.0.0.10. If 1.1.1.1 returns an address, the internet link carries DNS traffic and the name exists, which leaves your own server's upstream as the fault. If it also times out, look at the firewall for blocked port 53, on UDP (User Datagram Protocol) and TCP (Transmission Control Protocol).",
                new Wrong("Ping www.microsoft.com", "Ping has to resolve the name first through the same failing server, so it just fails the same way."),
                new Wrong("Run ipconfig /release and ipconfig /renew on the PC to get a fresh lease and a new set of DNS servers from DHCP (Dynamic Host Configuration Protocol)", "The lease would hand back the same server, 10.0.0.10, which is the correct one for this network."),
                new Wrong("Run nslookup www.microsoft.com again a few minutes later", "Repeating the same query to the same server can't show where the fault lies.")),
            new Step(
                "The public resolver answers straight away. On the DNS server, what do you check?",
                "The Forwarders list, with Get-DnsServerForwarder, testing each address with nslookup",
                "A Windows DNS server resolves outside names in one of two ways. It forwards the query to the resolvers listed on its Forwarders tab, or it uses root hints to work down from the root servers itself. Get-DnsServerForwarder lists the forwarder addresses and shows whether root hints are used as a fallback. Test each forwarder from the server with nslookup www.microsoft.com followed by its address. A forwarder that times out there is the break.",
                new Wrong("The corp.example zone's SOA (start of authority) record and its serial number, in case the zone has stopped loading on this server", "The server is answering for corp.example, so the zone is loaded and healthy."),
                new Wrong("The DHCP scope options", "Clients already receive 10.0.0.10 as their DNS server, and that is correct."),
                new Wrong("The hosts file on the server, for entries that override public names", "A hosts file only affects lookups that machine makes for itself, not the answers it gives to clients.")),
            new Step(
                "The forwarders are the old ISP's resolvers, and both time out. The root hints fallback is turned off. What is the fix?",
                "Replace the forwarders with resolvers that answer, clear the server's cache, and test",
                "ISP resolvers normally answer only that ISP's own customers, so they stopped responding when the office moved to the new provider. Set-DnsServerForwarder -IPAddress replaces the list, for example with the new ISP's resolvers or public ones such as 1.1.1.1 and 9.9.9.9. Clear-DnsServerCache then drops the failures the server has cached, and nslookup from a PC confirms that external names resolve. The change is made once on the server, and nothing has to be touched on the PCs.",
                new Wrong("Change the DHCP scope so that PCs use 8.8.8.8 as their DNS server", "A public resolver can't answer for corp.example, so the intranet, file shares and domain sign-in would break."),
                new Wrong("Add www.microsoft.com to each PC's hosts file", "A hosts entry fixes one name on one PC, and it goes wrong when the site's address changes."),
                new Wrong("Restart the DNS Server service every morning with a scheduled task until the old ISP's resolvers start answering the office again", "A restart doesn't change who the server forwards to, and those resolvers won't answer a non-customer."))),

        new(
            "dns-ttl-cached-old-record",
            ScenarioCategory.DnsTroubleshooting,
            "Old address served after a record change",
            "At 2 PM the company moved www.corp.example to a new web host and changed its A (address) record at the DNS (Domain Name System) provider. Two hours later some customers reach the new site, while the office and other customers still get the old server. The record's TTL (time to live) was 86400 seconds.",
            "Every DNS (Domain Name System) record carries a TTL (time to live), the number of seconds a resolver may keep the answer before asking again. Only the authoritative name servers hold the real record. Everyone else gets it through a recursive resolver, which serves its cached copy until the TTL runs out. What people call propagation is those caches expiring at different moments, which is why some users see the new address hours before others. To troubleshoot, ask the authoritative server directly: nslookup -type=NS corp.example lists the name servers, and nslookup www.corp.example followed by one of them returns the record as published. If that is right, the change worked and you are waiting on caches. The TTL in a resolver's answer counts down, so it shows how long that resolver will keep the old address. You can clear the caches you run, with Clear-DnsServerCache on a Windows DNS server and ipconfig /flushdns on a PC, but not other people's resolvers. Plan ahead for the next change: lower the TTL to about 300 seconds at least one full old TTL before the change, keep the old server running until the old TTL has passed, and raise the TTL again afterwards.",
            new Step(
                "What is the first thing to verify?",
                "Ask one of the domain's authoritative name servers for www.corp.example directly",
                "The authoritative name servers are the ones the DNS provider runs for corp.example, and they hold the record as it is actually published. nslookup -type=NS corp.example lists them, and nslookup www.corp.example ns1.dnshost.example asks one of them directly, with no cache in between. This tells you whether the change was saved correctly before you spend time anywhere else.",
                new Wrong("Reboot the new web server", "Customers who reach the new server get a working site, so the server isn't the problem."),
                new Wrong("Ping www.corp.example from several office PCs and from a few phones on mobile data, and compare the addresses that reply", "That shows what different caches hold. It can't tell you whether the published record is right."),
                new Wrong("Change the A record again to the same value so that it is published a second time", "Saving it again changes nothing if it is already correct, and resolvers aren't notified either way.")),
            new Step(
                "The authoritative server returns the new address. The office DNS server still returns the old one. What is happening?",
                "The office resolver is serving a cached answer, and will until its TTL runs out",
                "A recursive resolver, such as the office DNS server or an ISP (internet service provider)'s, looks a name up once and then keeps the answer for the TTL on the record, which was 86400 seconds, or 24 hours. Anyone who asked shortly before 2 PM cached the old address for up to a day. Resolvers that hadn't cached it, or whose copy has expired, fetch the new one. That is all propagation is. Nothing gets pushed out, and each cache expires at its own time.",
                new Wrong("The DNS provider hasn't finished saving the change", "The provider's own authoritative server is already returning the new address."),
                new Wrong("The old web host is still advertising the old address to resolvers, which overrides the new record until the hosting account is closed", "Web hosts don't publish DNS answers. Only the domain's authoritative name servers do."),
                new Wrong("The record is corrupted", "The authoritative server answers with the new address, and a damaged record couldn't.")),
            new Step(
                "How can you tell how much longer the office resolver will keep the old address?",
                "Run Resolve-DnsName www.corp.example and read the TTL in the answer",
                "An authoritative server always reports the full TTL. A resolver answering from its cache reports the time remaining, so the number falls each time you ask. Resolve-DnsName prints it in the TTL column, and dig and nslookup -debug show it too. If it reads 79200, the office resolver will hold the old address for another 22 hours unless its cache is cleared.",
                new Wrong("Check the domain's expiry date in the registrar's WHOIS record", "That is when the domain registration lapses. It has nothing to do with cached records."),
                new Wrong("Run tracert to the old server and count the hops, since each router on the way takes one off the TTL", "That is the TTL of an IP (Internet Protocol) packet, a hop limit. The DNS TTL is a cache lifetime in seconds, and the two only share a name."),
                new Wrong("Look at the Last Modified date on the old site's home page", "A file date on a web server says nothing about what a resolver has cached.")),
            new Step(
                "The office needs the new site now, and customers are still landing on the old server. What do you do?",
                "Clear the caches you control, and keep the old server running until the old TTL has passed",
                "You can only empty caches you run. Clear-DnsServerCache on the office DNS server and ipconfig /flushdns on the PCs make the office fetch the new address straight away. Browsers keep a short cache of their own, so restart them too. Customers' resolvers belong to their ISPs, and they will keep sending people to the old address for up to 24 hours from the change. Keep the old server answering, or have it redirect to the new one, until that time is up.",
                new Wrong("Shut the old server down so that resolvers are forced to look up the new address", "Resolvers don't check whether a server is alive. Customers with the cached address would just get an error."),
                new Wrong("Lower the record's TTL to 60 seconds now", "Resolvers that hold the old answer won't ask again until the old 24-hour TTL expires, so they don't see the new value in time."),
                new Wrong("Delete the A record, wait a few minutes, and create it again so that every resolver on the internet notices the change and refreshes", "Nothing notifies resolvers, and anyone who looks the name up during the gap caches a failure instead.")),
            new Step(
                "How should the next move be planned so that the switch takes minutes?",
                "Lower the TTL to about 300 seconds at least a day before the change, then raise it afterwards",
                "The TTL that matters is the one resolvers already hold. Lower it to 300 seconds at least one full old TTL ahead, which is 24 hours here, so every cached copy with the long TTL has expired by the time of the move. After the change, any resolver picks up the new address within five minutes. Once the new site is confirmed, put the TTL back up, since a longer TTL means fewer lookups and some protection if the DNS provider has a brief outage.",
                new Wrong("Set the TTL to 0 permanently, so that no resolver anywhere ever caches the record and every visitor always gets the current address", "Every visit would then need a fresh lookup, which slows the site, and some resolvers enforce a minimum anyway."),
                new Wrong("Make the change at night", "A 24-hour TTL outlasts the night, so daytime users would still get the old address."),
                new Wrong("Ask the DNS provider to push the new record out to ISPs' resolvers faster", "No such mechanism exists. Resolvers fetch a record again only when their copy expires."))),

        new(
            "dns-short-name-suffix",
            ScenarioCategory.DnsTroubleshooting,
            "Short names fail but full names resolve",
            "Since the company moved to a new VPN (virtual private network) client, remote staff can't open \\\\files01 or http://intranet. The same resources open in the office, and they open over the VPN when someone types the full name, such as files01.corp.example. The laptops aren't joined to the domain.",
            "DNS (Domain Name System) only holds FQDNs (fully qualified domain names). When a user types a single-label name such as files01, the Windows DNS client turns it into an FQDN by appending suffixes, then queries each result in turn. The suffixes come from the primary DNS suffix, which a domain-joined PC gets from its domain, from the connection-specific suffix of each adapter, set by DHCP (Dynamic Host Configuration Protocol) or by a VPN (virtual private network) profile, or from a DNS suffix search list, which replaces the other two when it is set. ipconfig /all shows all three. If none of them supplies corp.example, no query for files01.corp.example is ever sent. Windows then falls back to LLMNR (Link-Local Multicast Name Resolution) and NetBIOS broadcasts, which only reach the local network and don't cross a VPN. The test is to resolve the short name and the full name side by side with Resolve-DnsName. If only the full name works, the server and the record are fine and the client's suffixes are wrong. Fix it where the configuration comes from: the VPN profile for remote clients, DHCP option 15 for an office network, or the DNS suffix search list setting in Group Policy for domain PCs.",
            new Step(
                "A remote user is connected to the VPN. What comparison do you run on the laptop first?",
                "Resolve-DnsName files01, then Resolve-DnsName files01.corp.example",
                "The report already hints that short names fail and full names work, so test exactly that. Resolve-DnsName uses the Windows DNS client, the same path that File Explorer and browsers use. Running it with both forms of the name shows whether the VPN's DNS server can be reached and holds the record, and whether the laptop can turn the short name into the full one.",
                new Wrong("Ping the VPN gateway's public address", "The VPN is connected and full names open through it, so the tunnel is already known to work."),
                new Wrong("Uninstall the new VPN client and reinstall the old one on this laptop to see whether the problem follows the client", "It is a slow test that removes the configuration you need to inspect, and the old client is being retired."),
                new Wrong("Run ipconfig /flushdns and ask the user to try files01 again", "A flush removes cached answers. It can't supply an answer that was never resolved.")),
            new Step(
                "files01.corp.example resolves to 10.0.4.20. files01 returns 'DNS name does not exist'. What does that show?",
                "The DNS server and record are fine, and the laptop isn't appending corp.example",
                "DNS stores and answers full names only. The full name resolved, so the VPN is handing out a DNS server that works and the record for files01 is there. For a short name, the client has to build the full name itself by adding a suffix before it sends the query. The laptop isn't adding corp.example, so the question that reaches DNS is for a name that doesn't exist.",
                new Wrong("The files01 record is missing from the corp.example zone", "The full name is that record, and it resolved."),
                new Wrong("The VPN blocks short names", "A VPN carries packets. It has no view of how a name was typed before the query was built."),
                new Wrong("The file server registers its short name only with the DNS servers in the office, so remote clients can never see it", "DNS has no separate short-name records. Every record is stored under its full name.")),
            new Step(
                "Where do you see which suffixes the laptop adds to a short name?",
                "In ipconfig /all, under the primary suffix, the search list and each adapter's suffix",
                "ipconfig /all prints the three settings that matter. Primary Dns Suffix comes from domain membership. DNS Suffix Search List is the ordered list that the client will try. Connection-specific DNS Suffix appears under each adapter, including the VPN's, and is set by DHCP (Dynamic Host Configuration Protocol) or by the VPN profile. Get-DnsClientGlobalSetting and Get-DnsClient show the same values in PowerShell.",
                new Wrong("In the output of nslookup -type=SOA corp.example, which lists the suffixes that the zone tells clients to use", "The SOA (start of authority) record describes the zone. Zones don't tell clients which suffixes to add."),
                new Wrong("In the hosts file", "The hosts file maps names to addresses. It holds no suffix settings."),
                new Wrong("In the VPN server's connection log for this user", "The log shows when the user connected. Suffixes are a setting on the client.")),
            new Step(
                "The primary suffix is blank, the VPN adapter has no connection-specific suffix, and the search list holds only 'home', from the user's router. Why did short names work with the old client?",
                "The old VPN profile set corp.example as the adapter's DNS suffix, and the new one doesn't",
                "A laptop that isn't joined to the domain has no primary suffix, so the only source for corp.example was the VPN connection. The old client's profile supplied it as the connection-specific suffix, and the new profile left it out. Now the laptop tries files01.home, gets no answer, and falls back to LLMNR (Link-Local Multicast Name Resolution) and NetBIOS broadcasts. Those only reach the user's home network and never cross the VPN, so the lookup fails.",
                new Wrong("The old client used a faster DNS server", "Speed isn't the issue. The full name resolves at once through the current server."),
                new Wrong("The old client copied the company's hosts file to each laptop when it connected, and the new client has stopped doing that", "Nothing suggests a hosts file was in use, and the missing suffix explains the failure completely."),
                new Wrong("The old client joined the laptop to the domain each time it connected", "A VPN connection doesn't join a PC to a domain. That is a one-time change with its own account.")),
            new Step(
                "What is the right fix?",
                "Add corp.example as the DNS suffix in the VPN profile, so that every client receives it",
                "Fix it at the source of the setting. With corp.example as the VPN connection's DNS suffix, each laptop appends it while connected, and files01 becomes files01.corp.example before the query is sent. On the built-in Windows VPN client that is Set-VpnConnection -DnsSuffix, and other clients have the same setting in their profile or on the VPN server. One change reaches every user, and it goes away when the VPN disconnects. Check with ipconfig /all and by opening \\\\files01.",
                new Wrong("Tell staff to type the full name from now on", "Mapped drives, shortcuts and scripts already use the short names, and they would all stay broken."),
                new Wrong("Add files01 and intranet to the hosts file on every remote laptop, and update each file whenever a server's address changes", "It covers two names by hand on every laptop, and it breaks without warning when an address changes."),
                new Wrong("Create a zone named home on the company DNS server with copies of the records", "It would only suit users whose router hands out that suffix, and it leaves duplicate records to maintain."))),

        new(
            "dns-stale-record-dynamic-update",
            ScenarioCategory.DnsTroubleshooting,
            "Hostname resolves to the wrong computer",
            "Remote Desktop to PC-ACCT-07 by name fails for the help desk, and a ping to the name gets replies from an address that belongs to a different laptop. PC-ACCT-07 was reimaged last week, when its old computer account was deleted and the PC was joined to the domain again. PCs get their addresses from DHCP (Dynamic Host Configuration Protocol) and register themselves in the corp.example DNS (Domain Name System) zone.",
            "Windows PCs register their own A (address) records in DNS (Domain Name System) through dynamic update, and re-register them every 24 hours and whenever the address changes. A record goes stale when that update stops or is refused, and the name then points at an address that DHCP (Dynamic Host Configuration Protocol) has since leased to another device. Confirm it by comparing three things: the address DNS returns (nslookup), the address the PC really has (ipconfig), and who holds the address now (the DHCP lease list, or nslookup on the address for its PTR (pointer) record). On an Active Directory-integrated zone set to secure dynamic updates, each record has an ACL (access control list), and only the account that created the record may change it. A PC rejoined with a new computer account has a new SID (security identifier), so its updates to the old record are refused, and the DNS client logs event 8015 in the System log. Delete the stale record and run ipconfig /registerdns on the PC so that it creates a new one that it owns. For the long term, turn on aging and scavenging, which deletes dynamic records that haven't been refreshed. It has to be enabled on the zone and on at least one DNS server, and the default no-refresh and refresh intervals are 7 days each. Static records have no timestamp and are never scavenged.",
            new Step(
                "How do you confirm that DNS is pointing the name at the wrong machine?",
                "Compare the address from nslookup PC-ACCT-07 with the address ipconfig shows on that PC",
                "Two facts are needed: the address DNS gives out for the name, and the address the PC really has. nslookup PC-ACCT-07 returns the first, and ipconfig on the PC itself, or its lease in the DHCP console, gives the second. If they differ, the record is wrong. The DHCP lease list, or a reverse lookup with nslookup followed by the address, then shows which device holds that address now.",
                new Wrong("Ping PC-ACCT-07 again", "A reply only shows that something answers at that address. It doesn't say which machine it is."),
                new Wrong("Restart the Remote Desktop Services service on PC-ACCT-07 and ask the help desk to connect by name once more", "The connection never reaches PC-ACCT-07, so nothing on that PC affects it."),
                new Wrong("Run ipconfig /flushdns on the help desk PC and try again", "The wrong address comes from the DNS server, so the PC would just fetch the same answer again.")),
            new Step(
                "DNS returns 10.0.5.42 for PC-ACCT-07. The PC actually has 10.0.5.87, and DHCP shows 10.0.5.42 leased to a sales laptop. What is this?",
                "A stale A (address) record that still holds the address the PC had before the reimage",
                "An A record maps a name to an address. This one was written when PC-ACCT-07 held 10.0.5.42, before the reimage. The PC has since been given 10.0.5.87, and DHCP reused the old address for another laptop, but the record was never updated. A record that outlives the fact it describes is called stale. Windows PCs normally correct their own record whenever their address changes, so the next question is why this one didn't.",
                new Wrong("An IP (Internet Protocol) address conflict between the two computers", "Each PC has its own address and both work. Only the DNS record is wrong."),
                new Wrong("A rogue DHCP server", "Both leases came from the real server and appear in its lease list."),
                new Wrong("Round robin DNS, which rotates between the two addresses so that the load is shared across both of the computers", "Round robin needs several records under one name. Here there is one record, and it is wrong.")),
            new Step(
                "The zone accepts secure dynamic updates only, and the System log on PC-ACCT-07 has DNS client event 8015, a failed registration. Why can't the PC fix its own record?",
                "The record belongs to the deleted computer account, so the new account's update is refused",
                "With secure dynamic updates, every record in the zone has an ACL (access control list), and only the account that created it is allowed to change it. The old PC-ACCT-07 computer account created this record. Deleting that account and joining the PC again made a new account with a new SID (security identifier), even though the name is the same. The DNS server sees a different account trying to change someone else's record and refuses, and the client logs event 8015 each time it tries.",
                new Wrong("The DNS Client service is disabled on the PC", "A disabled service wouldn't attempt a registration, and event 8015 records an attempt that failed."),
                new Wrong("Reimaged PCs wait 30 days to register", "There's no such waiting period. A PC registers at startup and whenever its address changes."),
                new Wrong("The sales laptop registered the address first, and DNS allows only one name for each IP address in a zone", "Several names can point at one address. The laptop's own record doesn't block this one.")),
            new Step(
                "What fixes the name today?",
                "Delete the stale record in DNS Manager, then run ipconfig /registerdns on PC-ACCT-07",
                "An administrator can delete any record, so remove the old A record for PC-ACCT-07 from the corp.example zone in DNS Manager. Then ipconfig /registerdns on the PC makes it register straight away. With no old record in the way, the server lets the new computer account create one, and that account owns it from now on. Run ipconfig /flushdns on the help desk PC so that it drops the old answer, and confirm with nslookup that the name returns 10.0.5.87.",
                new Wrong("Set the zone to accept nonsecure and secure updates, so that any device is allowed to overwrite the record", "Any device could then overwrite any record, including a server's, which opens the zone to spoofing."),
                new Wrong("Give PC-ACCT-07 a static address of 10.0.5.42", "That address is leased to the sales laptop, so the two would conflict."),
                new Wrong("Add the correct address to the hosts file on the help desk PCs", "It hides the fault on a few PCs and leaves the wrong record for everyone else.")),
            new Step(
                "How do you stop stale records building up across the zone?",
                "Turn on aging for the zone and scavenging on one DNS server",
                "Every dynamic record carries a timestamp that its owner refreshes. Aging and scavenging uses it to delete records nobody has refreshed. With the default intervals, a record's timestamp can't be refreshed for the first 7 days, the no-refresh interval, and then it has 7 more days, the refresh interval, to be refreshed before it becomes eligible for deletion. It must be turned on in two places: aging on the zone, and scavenging on at least one DNS server. Records created by hand are static, have no timestamp, and are never removed. Check the timestamps before the first run, because a record that should have been static but was registered dynamically will be deleted too.",
                new Wrong("Shorten the DHCP lease to one hour", "Addresses would be reused sooner, so stale records would point at the wrong device more often."),
                new Wrong("Have a technician delete old records by hand each month, checking each one against the DHCP lease list", "It is slow and error-prone, and it leaves up to a month of stale records between passes."),
                new Wrong("Give every PC a static address so that its record never changes", "It gives up DHCP for the whole site, and retired PCs would still leave records behind."))),

        // ------------------------------------------------------------- Mac network issues
        new(
            "mac-dns-cache-mdnsresponder",
            ScenarioCategory.MacNetworkIssues,
            "Stale DNS cache on macOS",
            "Your team moved intranet.corp.example to a new server this morning and updated DNS (Domain Name System). Windows laptops reach the new site, but a MacBook still loads the old server. The Mac has a normal internet connection, and browsing elsewhere works fine.",
            "macOS caches DNS (Domain Name System) results in the mDNSResponder daemon, and most apps resolve names through that system resolver. Tools such as dig and nslookup query DNS servers directly and skip the system resolver, while dscacheutil -q host follows the path that apps use. That makes comparing them a good test: if dig shows the new address and dscacheutil shows the old one, the fault is on the Mac rather than in DNS. Clear it with sudo dscacheutil -flushcache followed by sudo killall -HUP mDNSResponder. The HUP (hang-up) signal tells the daemon to drop its cache without stopping it. If the old answer survives a flush, look at what the system resolver checks besides DNS, such as an entry in /etc/hosts, or a scoped resolver from a VPN (virtual private network), which scutil --dns will list.",
            new Step(
                "How do you tell whether this is a local cache or a real DNS problem?",
                "Compare dig intranet.corp.example with dscacheutil -q host -a name intranet.corp.example",
                "On a Mac there are two ways to look up a name, and they can disagree. dig sends a query straight to the DNS server and shows that server's answer. dscacheutil -q host goes through the macOS system resolver, mDNSResponder, which is what Safari and other apps use, and it includes the cache and local overrides. Running both tells you whether DNS is wrong or only this Mac is.",
                new Wrong("Run ipconfig /flushdns in Terminal and check the site again", "That's a Windows command. On macOS, ipconfig is a different tool and has no such option."),
                new Wrong("Run nslookup intranet.corp.example on the Mac, since it follows the same resolver path that apps use", "nslookup queries the DNS server directly, so it can't show you what the Mac's cache is holding."),
                new Wrong("Run ifconfig en0 to look at the Mac's interface details", "This shows the interface's addressing. It tells you nothing about how a name resolves.")),
            new Step(
                "dig returns the new IP (Internet Protocol), and dscacheutil returns the old one. What does that show?",
                "DNS is correct, and the Mac's system resolver is serving a stale answer",
                "dig's answer comes from the DNS server, and it's the new address, so the record is correct. dscacheutil's answer comes from the Mac's own resolver, and it's the old one. The gap between them is local to this Mac: a cached result, or something that overrides DNS. The Windows laptops already suggested the server side was fine, and this confirms it.",
                new Wrong("The authoritative DNS record hasn't been updated yet", "dig already got the new address, and Windows laptops are resolving it too."),
                new Wrong("The office router is intercepting DNS and rewriting responses for this one host on the Mac's subnet", "A rewriting router would affect dig as well, and Windows laptops on it work."),
                new Wrong("The web server is redirecting the Mac back to the old host", "dscacheutil shows the wrong IP before any web traffic happens.")),
            new Step(
                "Which command clears the DNS cache on macOS?",
                "sudo dscacheutil -flushcache; sudo killall -HUP mDNSResponder",
                "macOS keeps its DNS cache in the mDNSResponder process. dscacheutil -flushcache clears the Directory Service cache, and killall -HUP mDNSResponder sends the hang-up signal, which tells the daemon to throw away its cache and carry on running. Both need sudo. The command prints nothing when it works, so check by running dscacheutil -q host again.",
                new Wrong("sudo resolvectl flush-caches", "That's for systemd-resolved on Linux, which macOS doesn't use."),
                new Wrong("sudo launchctl disable system/com.apple.Finder; sudo killall Finder", "Finder isn't involved in DNS, and disabling system services can cause other problems."),
                new Wrong("sudo killall -9 networksetup", "networksetup is a command-line tool, not a running cache daemon.")),
            new Step(
                "After the flush, dscacheutil still returns the old IP, even though dig returns the new one. What do you check next?",
                "Look for an entry in /etc/hosts, then review the resolvers with scutil --dns",
                "The system resolver has sources other than DNS, and a flush doesn't touch them. The file /etc/hosts is read first, so a line for intranet.corp.example there, perhaps left from testing, overrides DNS permanently. scutil --dns lists every resolver the Mac uses, including ones a VPN (virtual private network) adds for specific domains, which may point at a DNS server that still has the old record.",
                new Wrong("Reinstall macOS to restore the default network configuration", "That's a drastic step for a problem that one text file or a resolver entry can explain."),
                new Wrong("Run the same flush command again until the result changes", "The cache is already clear, so repeating the flush won't change a persistent source."),
                new Wrong("Ask the DNS team to lower the record's TTL (time to live) and wait for the old answer to age out of the cache", "DNS is already answering correctly, so a TTL change can't affect this Mac's answer."))),

        new(
            "mac-8021x-auth-drops",
            ScenarioCategory.MacNetworkIssues,
            "802.1X Wi-Fi drops on MacBooks",
            "MacBooks on the WPA2-Enterprise (Wi-Fi Protected Access 2, Enterprise mode) SSID (service set identifier) CorpSecure started dropping off Wi-Fi about 8 hours after connecting, and they fail to rejoin. It started the day after the RADIUS (Remote Authentication Dial-In User Service) server's certificate was renewed. Windows laptops are fine. The Macs connect using a configuration profile from your MDM (mobile device management).",
            "802.1X authenticates a device before it's given network access. With PEAP (Protected Extensible Authentication Protocol) or EAP-TLS (Extensible Authentication Protocol with Transport Layer Security), the client validates the RADIUS (Remote Authentication Dial-In User Service) server's certificate during the TLS handshake. On macOS, the eapolclient process handles this, and an MDM (mobile device management) profile normally defines which certificates and server names to trust. Macs stay connected on an existing session, then fail when reauthentication happens, which explains drops long after the certificate change. Here the 8-hour pattern matches the session timeout. Windows machines were fine because they trust the new chain another way. The right fix is to push an updated profile containing the new CA (certificate authority) chain and trusted server names. Telling users to click Trust or turning off validation trains people to accept a fake access point, which is how credentials get stolen. On Windows NPS (Network Policy Server), event 6272 means access was granted and 6273 means it was denied.",
            new Step(
                "Where do you gather evidence first?",
                "Match the drop times in the Mac's eapolclient logs against the RADIUS server's logs",
                "802.1X has three parts. The Mac is the supplicant, the access point is the authenticator, and the RADIUS server decides. To see why it fails you need both ends. On the Mac, the eapolclient process writes the EAP (Extensible Authentication Protocol) conversation to the system log, which you can read with the log show command. The RADIUS server's log gives its reason for each accept or reject. Match the two at the times of the drops.",
                new Wrong("Forget the network on each Mac and then rejoin it manually", "This removes evidence and may not work on a network managed by a profile."),
                new Wrong("Replace the access points that the affected Macs are using, starting with the busiest floor of the office", "Windows laptops work on the same access points, so the hardware isn't the likely cause."),
                new Wrong("Check the DHCP (Dynamic Host Configuration Protocol) lease for each Mac on the staff VLAN (virtual local area network)", "802.1X happens before an IP (Internet Protocol) address is assigned, and these drops are authentication failures.")),
            new Step(
                "The Mac logs show a TLS (Transport Layer Security) failure during EAP, and RADIUS logs an unknown CA (certificate authority) alert from the client at each drop. Passwords are correct. What is the cause?",
                "The Macs' 802.1X profile doesn't trust the renewed RADIUS certificate chain",
                "In PEAP (Protected Extensible Authentication Protocol) and EAP-TLS (Extensible Authentication Protocol with Transport Layer Security), the RADIUS server presents its certificate first, and the client checks it before sending any credentials. The Mac's configuration profile lists exactly which CA and server names to trust. The renewed certificate comes from a chain that isn't in that list, so the Mac ends the handshake with an unknown CA alert. The password is never sent, so it can't be the problem.",
                new Wrong("The users' directory passwords expired overnight, so the RADIUS server is rejecting every login", "Expired passwords produce a credential rejection, and only after the TLS tunnel forms."),
                new Wrong("The Wi-Fi pre-shared key was rotated on the access points", "WPA2-Enterprise uses 802.1X per user and has no pre-shared key."),
                new Wrong("The DHCP scope for the staff VLAN has run out of addresses", "A device needs to authenticate first, so this failure comes before DHCP.")),
            new Step(
                "Why did the Macs only drop about 8 hours later, and not the moment the certificate changed?",
                "Certificates are only checked when authenticating, and reauthentication happens at session timeout",
                "The certificate is only checked during an authentication. Macs that were already connected kept their sessions after the renewal. The wireless controller or RADIUS server sets a session timeout, here 8 hours, and when it expires the client must authenticate again. That was the first time each Mac saw the new certificate, and that's when it failed. It's a pattern worth knowing: a change can break things at the next reauthentication, not at once.",
                new Wrong("macOS checks certificates for revocation on a fixed daily schedule", "Revocation isn't what's failing, since the client reports an unknown CA."),
                new Wrong("DNS (Domain Name System) caching delayed the new certificate from reaching the Macs", "The certificate is presented in the EAP exchange, not looked up through DNS."),
                new Wrong("The access points stored the old certificate in their memory", "The access point only relays EAP. The RADIUS server presents the certificate.")),
            new Step(
                "What is the correct fix?",
                "Push an updated MDM profile with the new CA chain and trusted server names",
                "The Macs' trust settings come from the MDM profile, so that's where the fix goes. Update the Wi-Fi payload with the new root and intermediate CA certificates and the RADIUS server's name under trusted server names, and push it. Every Mac picks it up without user action. Validation stays on, and validation is what stops a fake access point with the same SSID from collecting passwords.",
                new Wrong("Tell users to click Trust when the certificate prompt appears", "Teaching users to accept certificate prompts exposes them to fake access points."),
                new Wrong("Turn off server certificate validation in the 802.1X profile so the Macs accept the renewed certificate", "Without validation, Macs may send credentials to any rogue access point using that SSID."),
                new Wrong("Move the MacBooks to the open guest SSID for the time being", "This drops access to internal resources and leaves the failure in place.")),
            new Step(
                "How do you confirm the fix has worked?",
                "Confirm the profile is installed, then verify NPS (Network Policy Server) event 6272 through a full session timeout",
                "The failure only appeared at reauthentication, so a first connection doesn't prove the fix. Confirm the new profile is on the Macs, then watch the same Macs past a full 8-hour session. On NPS, event 6272 means access was granted and 6273 means it was denied. A 6272 for a Mac's reauthentication after the timeout, with no 6273, is the proof.",
                new Wrong("Check that a Mac connects again straight after the profile arrives, then close the ticket as resolved", "The first connection is useful, but it doesn't prove the 8-hour reauthentication passes."),
                new Wrong("Ask one user to say whether Wi-Fi feels more stable today", "A single impression can't prove a fault that shows up hours later."),
                new Wrong("Confirm the Wi-Fi icon in the menu bar shows full signal", "Signal strength has nothing to do with whether authentication succeeds."))),

        // --------------------------------------------------------------- Active Directory
        new(
            "ad-account-lockout-source",
            ScenarioCategory.ActiveDirectory,
            "Account keeps locking after a password change",
            "Dana changed her domain password on Monday. Since then her account locks out every 20 minutes or so, even while she's away from her desk. The help desk has unlocked it six times, and each unlock lasts only a few minutes.",
            "An account locks when the count of bad passwords passes the lockout threshold, so a lockout that repeats on a timer means something is still sending the old password. A domain controller that sees a bad password checks it with the PDC (primary domain controller) emulator, which always has the newest password, and lockouts are processed there. That makes the PDC emulator's Security log the place to look for event 4740, 'A user account was locked out'. Find that DC (domain controller) with (Get-ADDomain).PDCEmulator. The event's Caller Computer Name field names the computer the failed attempt came from. On that computer, event 4625 records each failed logon with its logon type: 2 is interactive, 3 is network, 4 is batch (scheduled tasks), 5 is service, and 10 is Remote Desktop. Usual culprits are scheduled tasks, services, mapped drives, saved entries in Credential Manager, disconnected Remote Desktop sessions, and mail apps on phones. Event 4740 is only logged when Audit User Account Management is enabled.",
            new Step(
                "The help desk wants to unlock the account a seventh time. What should happen first?",
                "Trace the source of the bad password attempts before unlocking the account again",
                "Each wrong password raises the account's badPwdCount attribute, and the account locks when the count reaches the lockout threshold within the observation window set in the password policy. Dana isn't at her desk when it happens and it repeats every 20 minutes, so a device or program is sending her old password on a schedule. Unlocking resets the counter, and the next few attempts lock it again. The real job is to find the source, and the logs can name it.",
                new Wrong("Reset Dana's password to something new and have her sign in again", "The stale credential is wrong against any new password, so the lockouts continue."),
                new Wrong("Raise the lockout threshold in the Default Domain Policy so that accounts take far longer to lock out", "This weakens password-guessing protection for every account to hide one fault."),
                new Wrong("Set Dana's password to never expire", "Expiry isn't involved. The password was already changed, and something kept the old one.")),
            new Step(
                "Where do you look to find out where the bad passwords are coming from?",
                "The Security log on the PDC (primary domain controller) emulator, filtered to event 4740 for Dana's account",
                "Every domain controller forwards a failed password to the PDC emulator, because it always holds the newest password, and lockouts are recorded there. Find it with (Get-ADDomain).PDCEmulator. In its Security log, event 4740, 'A user account was locked out', has a Caller Computer Name field with the computer the bad attempts came from. The event is only written if Audit User Account Management is turned on.",
                new Wrong("The Application log on Dana's laptop", "Lockout events are security events written by domain controllers, and her laptop may not be the source."),
                new Wrong("The System log on each file server Dana uses, starting with the one she opens most often during the day", "This guesses at the source. The lockout event names it directly."),
                new Wrong("The DNS (Domain Name System) Server log on the nearest domain controller", "DNS logs record name resolution, not authentication failures.")),
            new Step(
                "Event 4740 shows Caller Computer Name: RECEPTION-PC02, a shared front-desk PC Dana covered last week. What next?",
                "On RECEPTION-PC02, read the 4625 events to see which logon type is failing",
                "Event 4740 gave you the computer. Event 4625, 'An account failed to log on', in that computer's Security log gives you the kind of logon. Its Logon Type field tells you what is doing it: 2 is someone at the keyboard, 3 is a network connection such as a mapped drive, 4 is a scheduled task, 5 is a service, and 10 is Remote Desktop. The event also names the process.",
                new Wrong("Disable the RECEPTION-PC02 computer account in Active Directory", "That takes a working front-desk PC off the domain and tells you nothing about the cause."),
                new Wrong("Reimage RECEPTION-PC02, because a PC that keeps sending bad passwords has clearly been infected by malware", "A leftover saved credential is far more likely, and reimaging destroys the evidence either way."),
                new Wrong("Delete Dana's roaming profile from the profile server", "Nothing ties the profile to the failed attempts, and this risks losing her data.")),
            new Step(
                "The 4625 events for Dana's account on that PC all show Logon Type 4, about 20 minutes apart. What is the likely cause?",
                "A scheduled task on that PC saved under Dana's account with her old password",
                "Logon type 4 is a batch logon, which is what Task Scheduler uses to run a task as a stored account. The task saves the password when it's created and never learns about a change. Each run sends the old password, and a trigger every 20 minutes matches the lockout interval. Dana covered that PC last week, which is when she would have set it up.",
                new Wrong("Someone at the front desk keeps typing Dana's password incorrectly", "Typing at the keyboard is an interactive logon, type 2, and people don't fail on a 20-minute timer."),
                new Wrong("A mapped network drive on Dana's own laptop is reconnecting with the credentials it saved before Monday", "That would be a network logon, type 3, and the caller would be her laptop."),
                new Wrong("Dana's phone is sending the old password to the mail server", "The caller would then be the mail or sync server, not the front-desk PC.")),
            new Step(
                "You find a report-export task running as Dana. How do you finish the ticket?",
                "Fix or remove the task's saved credentials, unlock the account, then watch for new 4740 events",
                "Deal with the cause, then the symptom, then verify. Update the task's stored password, or better, remove it or run it under a service account so it isn't tied to a person. Then unlock Dana's account. Then watch the PDC emulator for new 4740 events for a few cycles. If any appear with a different caller computer, there's a second source, such as a phone or a mapped drive.",
                new Wrong("Unlock the account and close the ticket", "The task still holds the old password, so the account locks again at its next run."),
                new Wrong("Create a fine-grained password policy with a lockout threshold of 0 and apply it to Dana, so her account can't lock", "This removes lockout protection from her account and leaves the faulty task running."),
                new Wrong("Rename Dana's account so the task can no longer find it", "This breaks her sign-in and everything else tied to the name, to avoid editing one task."))),

        new(
            "ad-trust-relationship-failed",
            ScenarioCategory.ActiveDirectory,
            "Trust relationship with the domain failed",
            "After a bad software install, a technician reverted the virtual desktop VDI-ACCT07 to a checkpoint taken ten weeks ago. Now every domain user who tries to sign in to it gets 'The trust relationship between this workstation and the primary domain failed'.",
            "Every domain-joined computer has its own account in Active Directory, with a password that the computer and the domain both know. The computer uses it to build a secure channel to a domain controller, and domain sign-ins on that computer depend on the channel. By default the computer itself starts a password change every 30 days. Active Directory never expires a machine password, so a PC that was simply switched off for months still works. The usual real causes are restoring an old checkpoint or image, which brings back a password the domain has since replaced, or a computer account that was reset, deleted, or taken over by another machine with the same name. Test-ComputerSecureChannel reports whether the channel is healthy, and adding -Repair with a domain credential resets it in place. Reset-ComputerMachinePassword is the next option. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort. Don't delete the computer object, since a new one has a different SID (security identifier) and loses its group memberships.",
            new Step(
                "What does this error mean?",
                "The PC's secure channel is broken, as its machine account password doesn't match the domain's",
                "Every domain-joined PC has a computer account in Active Directory with its own password, which the PC changes by itself every 30 days. The Netlogon service uses that password to open a secure channel to a domain controller, and it needs the channel to check users' sign-ins. If the password stored on the PC doesn't match the one in Active Directory, the domain controller refuses the channel. Every domain user on that PC then gets this error.",
                new Wrong("The user's domain password has expired", "An expired user password gives a prompt to change it, and it wouldn't affect every user on one PC."),
                new Wrong("An administrator has removed the forest trust to a partner organization, so its accounts can no longer sign in here", "Despite the wording, this message is about the computer's own account, not a trust between domains."),
                new Wrong("The domain controller's certificate has expired", "That would affect many computers at once, not one reverted virtual desktop.")),
            new Step(
                "Domain sign-ins fail on this PC. How do you get in to repair it?",
                "Sign in with a local administrator account, typed as .\\Administrator",
                "A local account is stored on the PC itself, in the SAM (Security Account Manager) database, and it's verified there without any domain controller. That's why it still works when the secure channel is down. Use the built-in Administrator or a local admin managed by LAPS (Local Administrator Password Solution), signing in as .\\Administrator so Windows checks the local database.",
                new Wrong("Sign in as a member of Domain Admins, since that group's rights let it bypass the secure channel", "A domain admin is still a domain account, and it needs the same broken channel to be authenticated."),
                new Wrong("Keep retrying until a different domain controller answers", "Every domain controller holds the same machine account password, so none will accept this PC."),
                new Wrong("Sign in with the user's Microsoft account", "A domain-joined PC with no such account set up has nothing to check it against.")),
            new Step(
                "How do you confirm that the secure channel is the problem?",
                "Run Test-ComputerSecureChannel -Verbose in an elevated PowerShell window",
                "Test-ComputerSecureChannel checks whether the PC can authenticate to a domain controller with its machine account password. It returns True for a healthy channel and False for a broken one, and -Verbose adds the reason. nltest /sc_verify with the domain name does the same check. It turns the error message into a confirmed diagnosis before you change anything.",
                new Wrong("Ping the domain controller's IP (Internet Protocol) address", "A reply shows the network path works. It says nothing about whether the domain trusts this PC."),
                new Wrong("Run gpupdate /force and read through the output for computer policy errors, then compare it with a working PC", "Policy would fail, but only as a side effect. It doesn't test the channel itself."),
                new Wrong("Run ipconfig /flushdns and try the sign-in again", "The PC can already find the domain. What's wrong is what happens when it gets there.")),
            new Step(
                "The test returns False. What is the least disruptive fix?",
                "Run Test-ComputerSecureChannel -Repair -Credential with a domain account allowed to reset it",
                "With -Repair, the cmdlet sets a new machine password and writes it to both the PC and its account in Active Directory, using the domain credential you supply. The computer object, its SID (security identifier) and its group memberships stay as they are, and normally no restart is needed. Reset-ComputerMachinePassword is the alternative. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort.",
                new Wrong("Delete the computer object in Active Directory, then restart", "The PC then has no account at all, and a new object gets a new SID and loses its group memberships."),
                new Wrong("Move the PC to a workgroup, restart, join the domain again and restart once more, as that's the only supported repair", "Rejoining works, but it isn't the only way, and it costs two restarts for a problem one command can fix."),
                new Wrong("Reset the user's password in Active Directory", "User passwords were never the problem. The mismatch is on the computer's account.")),
            new Step(
                "The manager asks why reverting the checkpoint caused this. What do you say?",
                "The checkpoint restored an old machine password that the domain had since replaced",
                "The machine password changes every 30 days, and the checkpoint was ten weeks old. Since then the PC changed its password at least twice, and Active Directory stores the newest one. Reverting brought back the PC's copy from ten weeks ago, which the domain no longer accepts. Being switched off isn't the cause, because Active Directory never expires a machine password. Restoring old state is.",
                new Wrong("The VM (virtual machine) had been off for more than 30 days, so Active Directory expired its machine account password", "The computer starts its own password changes, and Active Directory doesn't expire machine passwords for being offline."),
                new Wrong("Too many users typed wrong passwords on that PC", "Bad user passwords lock user accounts. They don't affect the computer's own account."),
                new Wrong("The revert gave the VM (virtual machine) a different IP address", "The secure channel is based on the machine account password, not the address."))),

        new(
            "ad-replication-rpc-1722",
            ScenarioCategory.ActiveDirectory,
            "New accounts missing at a branch (replication error 1722)",
            "Accounts created this morning on HQ-DC01 can't sign in at the branch office, where PCs use the local domain controller BR-DC02. Password resets made at headquarters aren't working at the branch either. Internet access at the branch is fine.",
            "Domain controllers copy changes to each other, and when that stops, each site slowly works from a different version of the directory. Start with repadmin /replsummary for a forest-wide view of failures and how old they are, then repadmin /showrepl on the failing DC (domain controller) for each partition's last attempt, result, and last success. Error 1722, 'The RPC (remote procedure call) server is unavailable', is a connectivity error: the destination DC couldn't connect to the source DC. Microsoft notes that DNS (Domain Name System) lookup failures cause a large share of these, because DCs locate each other through records in the _msdcs zone. dcdiag /test:dns checks that, and nltest /dsgetdc tests the DC locator. A DC must use internal DNS servers that host the Active Directory zones. A public resolver doesn't hold those records, and an alternate server is only tried when the preferred one doesn't answer. Other causes of 1722 are blocked ports, namely TCP (Transmission Control Protocol) 135 for the endpoint mapper and the dynamic RPC range. Don't leave replication broken for long. A DC that stays out of sync past the tombstone lifetime, usually 180 days, can bring deleted objects back.",
            new Step(
                "Which command gives you the quickest overall view of replication health?",
                "repadmin /replsummary",
                "repadmin /replsummary contacts every domain controller and prints one line each, as a source and as a destination: how many replication attempts failed, the longest time since a success, and the error code. It shows in seconds which DC (domain controller) is behind and for how long. repadmin /showrepl on that DC then gives the detail for each partition and partner.",
                new Wrong("gpupdate /force", "This refreshes Group Policy on one computer. It doesn't report on replication between DCs."),
                new Wrong("netdom query fsmo", "This lists which DCs hold the operations master roles, not whether they are replicating."),
                new Wrong("dcpromo on BR-DC02, to demote it and promote it again so that it pulls down a completely fresh copy of the directory", "Rebuilding a DC before diagnosing it is drastic, and the new one would hit the same fault.")),
            new Step(
                "The summary shows BR-DC02 failing inbound replication for two days with error 1722. What does that error tell you?",
                "BR-DC02 can't connect to the RPC (remote procedure call) service on its source DC, so suspect DNS (Domain Name System) or the network path",
                "Replication is pull-based. BR-DC02 connects to HQ-DC01 over RPC and asks for changes. Error 1722, 'The RPC server is unavailable', means that connection couldn't be made. The causes are a name that doesn't resolve, a blocked port, which is TCP (Transmission Control Protocol) 135 for the endpoint mapper and then a dynamic port from 49152 to 65535, or a source server that is down. HQ-DC01 is serving headquarters, so look at DNS and the path between the sites.",
                new Wrong("The two DCs disagree about a password, so the krbtgt account needs to be reset", "Nothing in a 1722 error points to Kerberos keys, and resetting krbtgt is a major change."),
                new Wrong("The directory database on BR-DC02 is corrupt", "Database faults produce different errors. This one says the other server couldn't be reached."),
                new Wrong("BR-DC02 has no permission to replicate", "A permissions failure is reported as error 5 or 8453, access denied.")),
            new Step(
                "What do you test first?",
                "Run dcdiag /test:dns /v on BR-DC02 and check it can resolve HQ-DC01's records",
                "A domain controller finds its partner through DNS, using a CNAME (canonical name) record for the partner's GUID (globally unique identifier) in the _msdcs zone and then its host record. dcdiag /test:dns checks that the DC's DNS client settings are sound and that these records can be resolved and registered. DNS is the most common cause of 1722 and quick to test, so do it before looking at firewalls.",
                new Wrong("Restart HQ-DC01 during the working day", "The source DC is serving headquarters normally, and this adds an outage without a diagnosis."),
                new Wrong("Run repadmin /syncall over and over until the error stops appearing in the output", "Forcing a sync that can't connect just repeats the same failure."),
                new Wrong("Seize the operations master roles onto BR-DC02", "Seizing roles is for a role holder that is permanently gone. It isn't a replication fix.")),
            new Step(
                "The DNS test fails. BR-DC02's network adapter lists 8.8.8.8 as its preferred DNS server, set during an ISP (internet service provider) outage last week. What is the fix?",
                "Point BR-DC02 back at the internal AD (Active Directory) DNS servers, then clear its resolver cache",
                "The records that domain controllers need exist only on the internal DNS servers that host the Active Directory zones. 8.8.8.8 is a public resolver. It answers quickly that those names don't exist, and Windows accepts that answer and doesn't try the alternate server. Set the preferred DNS back to an internal DNS server, then run ipconfig /flushdns to clear the cached failures. Running ipconfig /registerdns and restarting Netlogon re-registers the DC's own records.",
                new Wrong("Add HQ-DC01's name and address to the hosts file on BR-DC02", "DCs find each other through several records in the _msdcs zone, which a single hosts entry can't replace."),
                new Wrong("Keep 8.8.8.8 as preferred and add an internal DNS server as the alternate, so that internet and domain names both resolve", "The alternate is only used when the preferred server doesn't answer, and 8.8.8.8 answers that the name doesn't exist."),
                new Wrong("Open TCP port 135 on the branch firewall", "The connection can't be attempted until the name resolves, and the DNS test already found the fault.")),
            new Step(
                "How do you confirm that replication has recovered?",
                "Force a sync, then check repadmin /replsummary for zero failures and a current last success",
                "Replication between sites runs on a schedule, so force it with repadmin /syncall /AdeP, or with Replicate Now in Active Directory Sites and Services. Then run repadmin /replsummary again. BR-DC02 should show 0 failures and a largest delta of minutes, not two days. As a final check, an account created this morning should exist on BR-DC02 and be able to sign in at the branch.",
                new Wrong("Check that BR-DC02 answers a ping from HQ-DC01", "Ping worked while replication was failing, so it can't show that anything changed."),
                new Wrong("Ask the new users to try again tomorrow, and reopen the ticket if any of them still can't sign in at the branch", "This leaves verification to users and a day's delay, when one command answers it now."),
                new Wrong("Look for a green status icon in Server Manager", "That reflects local services and events, not whether partitions are up to date."))),

        new(
            "ad-gpo-not-applying",
            ScenarioCategory.ActiveDirectory,
            "Group Policy isn't applying to a security group",
            "A colleague built a GPO (Group Policy Object) called Sales Drive Map with user settings and linked it to the Staff OU (organizational unit). To limit it to the sales team, they removed Authenticated Users from Security Filtering and added the Sales-Users group. Nobody in Sales gets the drive, and gpupdate /force doesn't help.",
            "A GPO (Group Policy Object) applies to a user only if it is linked where the user's account lives, and the user has both Read and Apply group policy permissions on it. Security Filtering is a shortcut for granting those two permissions. There's one more requirement that catches people out. Since security update MS16-072 in June 2016, Windows fetches user policy with the computer's security context, where it used to use the user's. The computer account therefore needs Read permission on any GPO that applies to the signed-in user. By default it gets that through Authenticated Users, a group that includes computer accounts. Removing Authenticated Users from Security Filtering takes away Read as well as Apply. The fix is on the GPO's Delegation tab: add Authenticated Users or Domain Computers with Read only. That lets computers read the GPO without applying it to everyone. On the client, gpresult /h produces a report of which GPOs were applied or denied, and why. A Deny entry always beats an Allow.",
            new Step(
                "What do you do first on an affected sales PC?",
                "Run gpresult /h report.html as the user and read which GPOs were applied or denied",
                "gpresult shows what Group Policy actually did on this PC for this user. The /h switch writes an HTML (Hypertext Markup Language) report that lists the applied GPOs, the denied GPOs with a reason such as security filtering or access denied, and the user's group memberships. Run it as the affected user. It replaces guessing in the management console with evidence from the client.",
                new Wrong("Restart the PC a few times", "If the policy can't be read, it fails the same way on every restart."),
                new Wrong("Run gpupdate /force on the domain controller that holds the PDC (primary domain controller) emulator role, and then wait 90 minutes", "Policy is processed on the client. Refreshing a DC (domain controller)'s own policy does nothing for a sales PC."),
                new Wrong("Map the drive by hand on each sales PC", "This hides the fault and leaves a GPO that still doesn't work for the next person.")),
            new Step(
                "The report doesn't list Sales Drive Map as applied, and it flags an access denied error for user policy. The user is in Sales-Users and in the Staff OU. What do you check?",
                "The GPO's Delegation tab, to see whether computer accounts can read it",
                "For a GPO to apply to a user, three things must hold. It's linked to an OU that contains the user, the user has Read and Apply group policy permission on it, and the PC can read it. The report shows the link, the OU and the group membership are right. Access denied is a permissions message, and the Delegation tab shows the GPO's full permission list.",
                new Wrong("Whether Block Inheritance is set on the Staff OU", "The GPO is linked to that OU directly. Block Inheritance only stops GPOs from higher up."),
                new Wrong("Whether the PC runs a newer Windows build than the domain controllers, since newer clients skip GPOs made on older servers", "There's no such rule. Clients process GPOs regardless of which server version created them."),
                new Wrong("The WMI (Windows Management Instrumentation) filter on the Default Domain Policy", "A WMI filter on another GPO has no effect on this one.")),
            new Step(
                "The Delegation tab lists Sales-Users, Domain Admins, Enterprise Admins, SYSTEM and Enterprise Domain Controllers. Why does that break the GPO?",
                "User policy is fetched in the computer's security context, and no entry gives the PCs Read",
                "Since the MS16-072 update in 2016, Windows downloads user policy using the computer account, where it used to use the user's. So the computer needs Read on the GPO, both on its object in Active Directory and on its files in SYSVOL. Normally it has that through Authenticated Users, which includes computer accounts. Removing that group from Security Filtering took away Read as well as Apply. Nothing in this list covers the sales PCs, so they can't read the GPO, and it's skipped.",
                new Wrong("Security filtering only accepts computer groups", "User groups are valid for filtering user settings. The missing piece is Read for computers."),
                new Wrong("Sales-Users is a global group, and filtering needs a domain local group", "Global security groups work for security filtering."),
                new Wrong("A GPO with no Authenticated Users entry in Security Filtering is treated as disabled, and every client in the domain skips it", "Filtering to other groups is supported. It just needs computers to keep Read access.")),
            new Step(
                "What is the right fix?",
                "On the Delegation tab, add Domain Computers or Authenticated Users with Read only",
                "Security Filtering grants two permissions together, Read and Apply group policy. The Delegation tab can grant Read by itself. Adding Domain Computers, or Authenticated Users, with Read lets every PC fetch the GPO. Only Sales-Users has Apply, so the drive still maps for the sales team alone.",
                new Wrong("Put Authenticated Users back into Security Filtering", "That grants Apply as well as Read, so the drive maps for everyone in the Staff OU again."),
                new Wrong("Add every sales user to the local Administrators group on their PC, so that their sessions are able to read the policy", "Local admin rights don't grant the computer account access to the GPO, and they create a security problem."),
                new Wrong("Set the GPO link to Enforced", "Enforced changes precedence over other GPOs. It can't make an unreadable GPO readable.")),
            new Step(
                "How do you confirm the fix?",
                "Run gpupdate /force on a sales PC, then confirm gpresult /r lists the GPO as applied",
                "Test the path that was failing: a sales user on a sales PC. gpupdate /force refreshes policy, and gpresult /r should then list Sales Drive Map under the user's applied GPOs, with the drive showing in File Explorer. A user outside Sales-Users shouldn't get the drive, which confirms the filtering still works.",
                new Wrong("Check in the console that the GPO link is enabled", "The link was enabled throughout the fault, so it proves nothing."),
                new Wrong("Assume it worked if nobody in Sales has called back about the missing drive by the end of the week", "Silence isn't evidence, and it leaves users without the drive in the meantime."),
                new Wrong("Confirm that an administrator's PC gets the drive", "An administrator outside Sales-Users shouldn't get it, so that test can only mislead."))),

        // ----------------------------------------------------------------- Windows Server
        new(
            "winsrv-system-drive-full",
            ScenarioCategory.WindowsServer,
            "System drive full on a web server",
            "Monitoring reports that the C: drive on WEB-SRV01 has 1% free space. Windows Update is failing on the server, and the web application has started logging write errors.",
            "A full system drive stops updates, logging, and sometimes the applications themselves, so act quickly, but measure before deleting anything. List the largest folders first. On servers the space usually goes to logs that nobody rotates, such as IIS (Internet Information Services) logs under C:\\inetpub\\logs\\LogFiles, along with dump files, temp folders and old installers. The WinSxS folder is the Windows component store. It holds the files Windows needs to service itself, and Microsoft warns that deleting files from it can leave a system unable to boot or update. Reduce it with the built-in tools. Dism /Online /Cleanup-Image /AnalyzeComponentStore reports how much can be reclaimed, and /StartComponentCleanup removes superseded component versions straight away, where the scheduled task waits 30 days. Adding /ResetBase removes every superseded version, and after that the updates already installed can't be uninstalled. Then fix the cause: put retention on the logs or move them to a data volume, and alert on free space well before it's critical.",
            new Step(
                "What do you do first?",
                "Find what is using the space by listing the largest folders on C:",
                "A full disk needs fast action, but deleting the wrong thing on a system drive can stop the server from booting. Measure first. A tool such as WinDirStat or TreeSize, or a PowerShell loop that totals folder sizes, shows the largest folders in minutes. That tells you what to remove, and it gives you the root cause for the ticket.",
                new Wrong("Delete the contents of C:\\Windows\\System32\\config", "That folder holds the registry hives. Removing them stops Windows from starting."),
                new Wrong("Restart the server to clear out temporary files", "A restart frees little, interrupts the site, and may not complete cleanly on a full disk."),
                new Wrong("Extend the C: volume straight away and close the alert, since every server needs a bigger disk as it gets older", "Without finding the cause, the new space fills the same way and the alert returns.")),
            new Step(
                "You find 38 GB of IIS (Internet Information Services) logs going back four years in C:\\inetpub\\logs\\LogFiles, and a 14 GB WinSxS folder. What is the best first move?",
                "Archive or delete the old IIS logs, keeping whatever the app owner must retain",
                "Take the biggest and safest item first. IIS writes a log file per site per day under C:\\inetpub\\logs\\LogFiles and never deletes them, so they grow without limit. They're only records, so removing old ones can't harm Windows or the site. Ask the application owner or security how long logs must be kept, archive what's needed to another volume, and delete the rest. That frees up to 38 GB.",
                new Wrong("Delete the WinSxS folder", "WinSxS is the component store. Deleting it can leave the server unable to boot or install updates."),
                new Wrong("Turn off the page file and delete pagefile.sys", "This risks out-of-memory failures and removes the ability to capture a crash dump."),
                new Wrong("Stop the World Wide Web Publishing Service so no more logs can be written while you decide what to do", "This takes the website down, and the logs already on disk still fill the drive.")),
            new Step(
                "How do you safely reduce the WinSxS folder?",
                "Run Dism /Online /Cleanup-Image /AnalyzeComponentStore, then /StartComponentCleanup",
                "WinSxS is the component store, which holds every version of Windows components so that updates can be installed and rolled back. It must only be cleaned with DISM (Deployment Image Servicing and Management). /AnalyzeComponentStore reports its real size and whether cleanup is recommended. /StartComponentCleanup then removes versions that newer updates have replaced. Deleting files in it by hand can leave Windows unable to update or boot.",
                new Wrong("Delete its oldest subfolders in File Explorer", "Windows tracks those files. Removing them by hand breaks servicing and can stop the server booting."),
                new Wrong("Run sfc /scannow", "System File Checker repairs protected system files. It doesn't remove superseded components."),
                new Wrong("Take ownership of the folder from TrustedInstaller, then turn on NTFS (New Technology File System) compression for it from the Properties page", "Changing ownership of the component store breaks servicing, and the store already compresses unused components.")),
            new Step(
                "A colleague suggests adding /ResetBase to the cleanup. What is the trade-off?",
                "It removes every superseded component version, so installed updates can't be uninstalled",
                "A normal cleanup keeps what's needed to uninstall recent updates. /ResetBase removes every superseded version, which frees more space but makes the installed updates permanent. If one of them later turns out to cause a problem, it can't be rolled back. Updates installed afterwards can still be uninstalled. On a production server, use it only when the installed updates are known to be stable.",
                new Wrong("It resets Windows to its original image and removes installed roles", "It only affects superseded component versions. Roles, features and data are untouched."),
                new Wrong("It clears the update history, so the server has to download and reinstall every update released since it was built", "Installed updates stay installed. What's lost is the ability to roll them back."),
                new Wrong("There is none, so always use it", "Losing the ability to uninstall a bad update matters on a production server.")),
            new Step(
                "Space is back to 45% free. How do you stop this from happening again?",
                "Set log retention or move logs off C:, and alert on free space much earlier",
                "Fix the cause, or the disk fills again. Set a retention for the IIS logs, for example a scheduled task that deletes files older than 30 or 90 days, or move the log directory to a data volume in the IIS logging settings. Then set monitoring to warn at around 20% free, so there's time to act before updates and the application start failing.",
                new Wrong("Schedule a restart every night", "Restarts don't delete logs, and they add downtime for no benefit."),
                new Wrong("Turn off IIS logging on the server completely, because nobody has read those log files in four years", "The logs are needed for troubleshooting and security investigations. Rotate them instead."),
                new Wrong("Disable Windows Update to stop WinSxS growing", "This leaves the server unpatched to save a small amount of space."))),

        new(
            "winsrv-rdp-cannot-connect",
            ScenarioCategory.WindowsServer,
            "Remote Desktop can't connect to a server",
            "Since last night's security hardening change, no administrator can open a Remote Desktop session to FILE-SRV03. The client reports 'Remote Desktop can't connect to the remote computer'. The server answers ping, and its file shares still work. You have access to its Hyper-V console.",
            "This error means the client never reached a working RDP (Remote Desktop Protocol) listener, and Microsoft's guidance splits the causes into the listener and the network. With console access, run qwinsta on the server. A line for rdp-tcp in the Listen state means the listener is up. If it's missing, check that fDenyTSConnections is 0, both under HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server and in the policy path if one is set, and that the Remote Desktop Services service (TermService) is running. Next, connect from the console with mstsc to localhost. If that works, the server side is healthy and the fault is between client and server. Test-NetConnection with -Port 3389 from another machine tests the TCP (Transmission Control Protocol) port itself, which ping can't do because ping uses ICMP (Internet Control Message Protocol). A False result points to the Windows Firewall rules named Remote Desktop - User Mode (TCP-In and UDP-In), a network firewall, or a changed port number. Failures after that point, such as credentials, Network Level Authentication, certificates, or membership of Remote Desktop Users, produce different messages because the connection has already been made.",
            new Step(
                "The server is running. Where do you start?",
                "Sign in at the console and run qwinsta to check for an rdp-tcp listener in the Listen state",
                "RDP (Remote Desktop Protocol) needs a listener on the server, which by default waits on TCP (Transmission Control Protocol) port 3389. qwinsta, short for query session, lists sessions and listeners. A line for rdp-tcp with the state Listen means the Remote Desktop service is up and accepting connections. If it's missing, the fault is on the server: RDP is disabled (fDenyTSConnections set to 1) or the TermService service is stopped. If it's there, the fault is between the client and the listener.",
                new Wrong("Restart the server from Hyper-V Manager", "A restart interrupts file shares that are working and won't undo a configuration change."),
                new Wrong("Remove the Remote Desktop Services role and add it again", "Administrative RDP doesn't use that role, and reinstalling skips the diagnosis."),
                new Wrong("Delete each administrator's saved .rdp file and clear the cached credentials in Credential Manager on their workstations", "Every administrator fails the same way after a server change, so the clients aren't the cause.")),
            new Step(
                "qwinsta shows rdp-tcp in the Listen state. How do you tell a server fault from a network fault?",
                "Connect with mstsc to localhost from the console, then test port 3389 from another PC",
                "Test from the inside outwards. mstsc to localhost on the server uses the RDP service without crossing the network or the firewall, so if it works, the service is healthy. Then, from another PC, Test-NetConnection FILE-SRV03 -Port 3389 tries a TCP connection to the RDP port. TcpTestSucceeded: True means the port is reachable, and False means something is blocking it. Ping can't answer this, because it uses ICMP (Internet Control Message Protocol) and not TCP 3389.",
                new Wrong("Ping the server from an administrator's PC", "Ping uses ICMP and already works. It can't show whether TCP 3389 is reachable."),
                new Wrong("Run tracert to the server and count the hops along the way to see whether the route has changed since yesterday", "Routing is fine, because ping and file shares work. The question is one TCP port."),
                new Wrong("Check that the administrators' passwords haven't expired", "An expired password produces a credentials message after connecting, not this error.")),
            new Step(
                "The localhost session works. From a PC on the same subnet, the port test returns TcpTestSucceeded: False. What does this show?",
                "Something is blocking TCP 3389 before the listener, most likely the server's own firewall",
                "Put the results together. The service works locally, and the port can't be reached from a PC on the same subnet. Same subnet means no router or network firewall sits in between. The only thing left between that PC and the listener is Windows Firewall on the server itself, and a hardening change the night before is a likely reason for a disabled rule.",
                new Wrong("The Remote Desktop Services service has stopped", "The localhost session used that service a moment ago, so it's running."),
                new Wrong("The administrators aren't members of the Remote Desktop Users group", "Group membership is checked after the connection is made, so the port test would succeed."),
                new Wrong("The server's Remote Desktop certificate has expired, which stops the TCP connection from being opened in the first place", "Certificates are exchanged after the TCP connection opens, so a bad one can't fail a port test.")),
            new Step(
                "In Windows Defender Firewall on the server, the rule Remote Desktop - User Mode (TCP-In) is disabled. What is the fix?",
                "Enable the Remote Desktop firewall rule group for the profiles the server uses",
                "Windows has built-in firewall rules for Remote Desktop, in the Remote Desktop group, including Remote Desktop - User Mode (TCP-In) for port 3389. Enabling that group for the active profile, usually Domain, allows RDP and nothing else. In PowerShell: Get-NetFirewallRule -DisplayGroup \"Remote Desktop\" | Set-NetFirewallRule -Enabled True. The broader options work too, but they open far more than the one port.",
                new Wrong("Turn Windows Firewall off on all profiles and leave it off", "Turning it off briefly can confirm a diagnosis, but leaving it off removes protection from every port."),
                new Wrong("Change the RDP port to 3390 in the registry so that the sessions avoid the rule that was disabled", "No rule allows 3390 either, so the connection is still blocked."),
                new Wrong("Add a rule that allows all inbound TCP ports", "This opens the whole server to fix one port.")),
            new Step(
                "How do you finish the job?",
                "Repeat the port test, connect by RDP, and correct the hardening change so it can't recur",
                "Repeat the tests that failed. Test-NetConnection on port 3389 should return True, and an administrator should be able to open a session from their own PC. Then find what disabled the rule. If the hardening script or GPO (Group Policy Object) still does it, the rule goes off again at the next run or policy refresh. Fix it at the source, with an exception that allows RDP from the admin network.",
                new Wrong("Confirm that the server still answers ping", "Ping answered throughout the fault, so it shows nothing about RDP."),
                new Wrong("Connect once from the Hyper-V console and close the ticket, since the console is the more secure way in anyway", "The console worked all along. The ticket is about remote sessions."),
                new Wrong("Email the administrators that it should be fixed", "Nothing has been tested, and the hardening change may reapply at its next run."))),

        new(
            "winsrv-service-logon-failure",
            ScenarioCategory.WindowsServer,
            "Service fails to start with a logon failure",
            "After Sunday's patching restart, the Contoso Sync service on APP-SRV02 won't start. Starting it by hand returns 'Error 1069: The service did not start due to a logon failure'. The service runs as the domain account CORP\\svc-sync, and the security team rotated service account passwords last week.",
            "A Windows service that runs as a user account stores that account's password in its own configuration, and Windows uses it to sign the account in each time the service starts. Error 1069 is the general message. The Service Control Manager writes the specific reason to the System event log. Event 7038 says the service was unable to log on with the currently configured password, followed by the cause: the user name or password is incorrect, the account is disabled, the account is locked out, the password must be changed, or the domain could not be contacted. Event 7041 is a different problem, where the account lacks the Log on as a service right. Changing a password in Active Directory doesn't update services that use it. A service that is already running keeps its session, so the mismatch only shows at the next start, often after a patching restart days later. Fix it by entering the new password on the service's Log On tab. Repeated start attempts with the old password can lock the account. A group managed service account avoids all this, because Active Directory rotates its password and the host retrieves it automatically.",
            new Step(
                "Where do you find the exact reason for the failure?",
                "The System event log, in Service Control Manager events such as 7038 and 7041",
                "Error 1069 only says that the service's account couldn't log on. The Service Control Manager writes the reason to the System event log. Event 7038 names the account and gives the cause: wrong password, account disabled, account locked out, or password must be changed. Event 7041 means the account lacks the 'Log on as a service' right. Filter the System log by the source Service Control Manager.",
                new Wrong("The Setup log, because the fault appeared straight after patching and a failed update is the likeliest cause", "The message says logon failure. The restart only exposed it, and the Setup log won't explain a logon."),
                new Wrong("The vendor's installation log from two years ago", "It describes how the service was installed, not why it can't sign in today."),
                new Wrong("The DNS (Domain Name System) Server log on a domain controller", "Name resolution isn't mentioned anywhere in this error.")),
            new Step(
                "Event 7038 says the service was unable to log on as CORP\\svc-sync with the currently configured password, because the user name or password is incorrect. What does this mean?",
                "The password saved in the service no longer matches the account's password in the domain",
                "A service that runs as a user account stores that account's password on the server, encrypted as an LSA (Local Security Authority) secret. At each start, Windows signs the account in with the stored password. The security team changed the password in Active Directory last week, and nobody updated the copy stored in the service. Event 7038 says exactly that: the currently configured password is incorrect.",
                new Wrong("The account lacks the Log on as a service right", "That is event 7041, whose text says the user has not been granted the requested logon type."),
                new Wrong("The account has been disabled", "Event 7038 has separate wording for that, saying the account is currently disabled."),
                new Wrong("An update replaced the service's program file, and the Service Control Manager can no longer verify its signature", "The event describes a logon with a password. It says nothing about the program file.")),
            new Step(
                "The password was rotated a week ago. Why did the service only fail on Sunday?",
                "A running service keeps its logon session, so the saved password is only used at the next start",
                "The stored password is only used when the service starts. The service started before the rotation and stayed signed in, so it kept running on its existing session. Sunday's patching restart stopped it, and the next start was the first time the old stored password was tried. This gap between the change and the failure is why password rotations cause outages days later.",
                new Wrong("Password changes take about a week to replicate between domain controllers", "Password changes replicate urgently, within seconds or minutes."),
                new Wrong("Windows keeps service passwords in a cache for seven days, then asks a domain controller for the new one", "Windows never fetches a new password for an ordinary account. An administrator has to enter it."),
                new Wrong("The update reset every service password", "Windows updates don't change the credentials saved for services.")),
            new Step(
                "What is the right fix?",
                "Enter the new password on the service's Log On tab, then start the service",
                "Open services.msc, go to the service's properties, and on the Log On tab enter the new password for CORP\\svc-sync. Then start the service. From the command line it's sc.exe config with obj= and password=. Don't keep retrying the start with the old password, because each attempt is a failed logon and can lock the account.",
                new Wrong("Switch the service to Local System", "Local System has full control of the server and none of the account's permissions on other systems."),
                new Wrong("Set svc-sync's password in Active Directory back to the old value, without involving the security team", "This reverses a security control and breaks anything already updated to the new password."),
                new Wrong("Change the startup type to Automatic (Delayed Start)", "A delayed start helps when the domain can't be contacted at boot. Here the password is wrong.")),
            new Step(
                "How do you stop the next password rotation from causing the same outage?",
                "Move the service to a group managed service account, whose password is handled automatically",
                "A group managed service account, or gMSA, is an Active Directory account built for services. Active Directory generates and rotates its password automatically, every 30 days by default, and the servers allowed to use it retrieve the password themselves. Nobody types it, and no stored copy goes stale. It needs a KDS (Key Distribution Service) root key in the domain and a service that supports running as a gMSA.",
                new Wrong("Set the password to never expire and exclude the account from every future rotation", "An unchanging password on a service account is exactly the risk rotation exists to remove."),
                new Wrong("Save the password in a text file on the server", "Anyone who can read the file can then use the account."),
                new Wrong("Add svc-sync to the Domain Admins group", "Group membership doesn't update a saved password, and this grants enormous privileges."))),

        new(
            "winsrv-share-access-denied",
            ScenarioCategory.WindowsServer,
            "Access denied on a file share",
            "Tomas joined Accounting today and gets 'Access is denied' when he opens \\\\FILE-SRV01\\Finance. His colleagues can open it. He was added to the Finance-RW group an hour ago and has been signed in since this morning.",
            "A shared folder has two sets of permissions. Share permissions apply only to access over the network, and NTFS (New Technology File System) permissions apply to every kind of access. Over the network both are evaluated and the more restrictive result wins, so Full Control on the share with Read in NTFS gives Read. Common practice is to keep share permissions broad, for example Authenticated Users with Full Control or Change, and to do the real control in NTFS with groups. Within one set, a user's permissions from all their groups add up, but a Deny entry beats any Allow. The Effective Access tab under Advanced Security Settings calculates the NTFS result for a named user. Group membership has its own catch. A user's access token, which lists their groups, is built when they sign in. Adding someone to a group changes Active Directory, but not a token that already exists, so the user has to sign out and back in. whoami /groups shows the groups in the current token. Grant access to groups and not to individual accounts, or the permissions become unmanageable.",
            new Step(
                "Tomas opens \\\\FILE-SRV01\\Finance over the network. Which permissions does the server check, and how do they combine?",
                "Share permissions first, then NTFS (New Technology File System) permissions, and he gets only what both sets allow",
                "A network connection to \\\\FILE-SRV01\\Finance passes two gates. First the SMB (Server Message Block) server checks the share permissions on the Finance share, which only apply to access over the network. Then it checks the NTFS permissions on the folder itself, which apply to every kind of access. Windows works out his total in each set from all his groups, and the more restrictive of the two totals is what he gets. Full Control on the share with Read in NTFS gives Read, and Read on the share with Modify in NTFS also gives Read. For this ticket it means a block in either set is enough to cause 'Access is denied', so you have to look at both.",
                new Wrong("Share permissions override NTFS (New Technology File System) permissions", "Neither set overrides the other. Each can only narrow the result."),
                new Wrong("Whichever of the two is more generous applies", "It's the reverse. Access is limited to what both sets allow."),
                new Wrong("NTFS (New Technology File System) permissions are skipped for network access, and they only apply to people signed in at the server itself", "NTFS permissions apply to every kind of access. It's share permissions that only apply over the network.")),
            new Step(
                "Where do you look to see what access Tomas should have?",
                "On the server, review the share permissions and the folder's Security tab, with Effective Access",
                "Check both sets on the server. Share permissions are on the folder's Sharing tab under Advanced Sharing > Permissions, or from Get-SmbShareAccess -Name Finance. NTFS permissions are on the Security tab. Under Advanced, the Effective Access tab works out the NTFS result for a user you pick, adding up all the groups they belong to and applying any Deny entries. It reads his membership from Active Directory, so it shows what he should get, which can differ from what his current session has.",
                new Wrong("In the Default Domain Policy", "Domain policy doesn't hold permissions for individual folders."),
                new Wrong("In the folder's Previous Versions tab", "That lists shadow copies for restoring files. It has nothing to do with permissions."),
                new Wrong("In the output of chkdsk on the data volume, in case file system errors have damaged the permissions on that folder", "His colleagues can open the same folder, so the file system isn't at fault.")),
            new Step(
                "The share grants Authenticated Users Full Control, and NTFS grants Finance-RW Modify. On Tomas's PC, whoami /groups doesn't list Finance-RW. What is the cause?",
                "His access token was built when he signed in, before he was added to the group",
                "The permissions are right. The share lets any authenticated user through with Full Control, and NTFS gives Modify to Finance-RW, so NTFS decides and members get Modify. Tomas is a member in Active Directory, but whoami /groups shows his current access token, and the group isn't in it. Windows builds the token once, at sign-in, from the groups he had at that moment. He signed in this morning and was added an hour ago. The token carries the SID (security identifier) of every group he belongs to, and the file server compares those SIDs with the entries in the folder's ACL (access control list). His token has no SID for Finance-RW, so no entry grants him access and the server denies it.",
                new Wrong("NTFS permissions take up to 24 hours to reach new group members", "There's no such delay. The permission is on the group, and his token doesn't carry the group."),
                new Wrong("The share has to be removed and created again every time the membership of one of its groups changes", "Shares refer to groups by their identifier, so membership changes need nothing done to the share."),
                new Wrong("Replication is failing between FILE-SRV01 and his PC", "Member servers and PCs don't replicate the directory. Only domain controllers do.")),
            new Step(
                "What is the fix?",
                "Have Tomas sign out and sign back in, so that Windows builds a new access token",
                "A new sign-in makes Windows build a new access token and request new Kerberos tickets, and the group SIDs inside them now include Finance-RW. Nothing on the server needs to change, because the permissions were already correct. Afterwards, whoami /groups should list Finance-RW, and the share should open. Running klist purge can refresh the tickets without a sign-out, but signing out and in is the reliable answer.",
                new Wrong("Grant Tomas's own account Full Control on the folder", "It bypasses the group, gives him more than his colleagues have, and leaves a one-off entry to maintain."),
                new Wrong("Add Everyone with Full Control in NTFS", "This opens the Finance folder to every user on the network."),
                new Wrong("Restart FILE-SRV01, so that every connected user is forced to collect fresh group memberships from the domain", "The stale token is on his own session, so a server restart interrupts everyone and fixes nothing.")),
            new Step(
                "Later, auditors need read-only access to the same folder. How do you set it up?",
                "Give an Auditors group Read & execute in the folder's NTFS permissions",
                "Use the same model as before: the share stays broad, and NTFS does the real control. The share gives Authenticated Users Full Control, so the NTFS entry decides what auditors get. Read & execute lets them open and list files but not change them. Grant it to a group, not to named people, so access follows membership as auditors come and go.",
                new Wrong("Add a Deny Full Control entry for the auditors on the share", "Denying Full Control denies every permission, including Read, and Deny beats any Allow."),
                new Wrong("Make a copy of the folder for the auditors", "The copy goes out of date immediately, and it doubles the sensitive data to protect."),
                new Wrong("Create a second, read-only share for the same folder and grant the auditors Modify in NTFS so they can open it", "They could still reach the original share, where NTFS Modify would let them change files."))),
    ];
}
