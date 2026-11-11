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
                "The problem appears to be on the service side, and message trace is the record of how Exchange Online handled the message.",
                new Wrong("Rebuild Priya's Outlook profile", "Her mail client isn't implicated. Internal mail works, and a new profile can't reveal what happened to a message that never reached the mailbox."),
                new Wrong("Reset Priya's password and revoke her sessions", "Nothing suggests a compromised account, and sign-in state doesn't affect inbound delivery."),
                new Wrong("Ask the supplier to resend the invoice from a personal mailbox, so that Priya at least gets it today", "This sidesteps the problem without finding the cause, and it teaches staff to accept invoices from unfamiliar addresses.")),
            new Step(
                "Message trace is open. How do you scope the search?",
                "Set the sender to billing@northwind-supply.example, the recipient to Priya, and a date range covering the missing invoices",
                "Filtering on both parties and the right window returns only the relevant messages, and the sender and recipient appear exactly as Exchange Online recorded them.",
                new Wrong("Search with Priya as the sender over the last 24 hours", "That finds what Priya sent, not what she should have received."),
                new Wrong("Run an unfiltered trace for the whole organization", "It returns a large volume of unrelated results and may hide the message you need."),
                new Wrong("Run a content search for 'invoice' across Priya's mailbox in Microsoft Purview, using the supplier's domain and a date range covering the missing invoices", "Content search only looks at items stored in mailboxes and other locations, so it can't find a message that was never delivered.")),
            new Step(
                "The trace shows the supplier's messages with the status Quarantined, flagged as high confidence spam. What is your next step?",
                "Open Quarantine in the Microsoft Defender portal, review the messages, and release them if they are legitimate",
                "Quarantined messages are held in the Defender portal. Reviewing them first confirms that they really are from the supplier before they reach the user.",
                new Wrong("Tell Priya to look in her Deleted Items folder", "A quarantined message was never delivered to a mailbox folder, so it won't be there."),
                new Wrong("Disable the anti-spam policy for the tenant", "This removes protection for everyone to fix a false positive affecting a single sender."),
                new Wrong("Ask the supplier to resend the invoices, then wait for the new copies to clear filtering and confirm that Priya has them", "The same filter verdict will probably apply again, and the held invoices remain stuck.")),
            new Step(
                "The messages were legitimate. How do you stop this from recurring?",
                "Report them as false positives through Submissions in the Defender portal and create a Tenant Allow/Block List allow entry from the submission",
                "Submitting teaches the filters, and the allow entry is targeted and expires, so it is the Microsoft-recommended way to handle a false positive.",
                new Wrong("Add northwind-supply.example to the allowed domains in the anti-spam policy that covers Priya, so future mail from the supplier isn't filtered as spam again", "Allowed domains are a known risk because spoofed mail from the domain can bypass spam filtering."),
                new Wrong("Create a mail flow rule that sets SCL to -1 for all external mail", "This bypasses spam filtering for all inbound mail, not just this supplier."),
                new Wrong("Add the supplier to Priya's Safe Senders in Outlook", "This doesn't reliably affect high confidence spam verdicts, and it helps only one mailbox."))),

        new(
            "email-forwarding-blocked",
            ScenarioCategory.EmailDelivery,
            "Auto-forward bounces with 5.7.520",
            "Marco, a contractor manager, set an inbox rule to forward project mail to a partner's external address. Every forwarded message now bounces, and the bounce message includes the code 5.7.520.",
            "NDR 5.7.520 means 'Access denied, your organization does not allow external forwarding'. Automatic external forwarding is controlled by the outbound spam filter policy, whose default setting (Automatic - System-controlled) behaves as Off. Attackers often use forwarding rules to exfiltrate mail from compromised mailboxes, so leave the Default policy alone and enable forwarding only through a custom policy scoped to the people with an approved business need. Check any remote domain settings as well, because they can also stop automatic forwarding.",
            new Step(
                "Before changing anything, what does the code 5.7.520 tell you?",
                "Automatic forwarding to an external recipient was blocked by your own organization's policy",
                "The text of this NDR is 'Access denied, your organization does not allow external forwarding', so the block is in your tenant rather than at the partner.",
                new Wrong("The partner's mail server rejected Marco's domain, which is why the bounce came back with a 5.7 code", "The block happens on your side of the connection before the message leaves Exchange Online."),
                new Wrong("Marco's mailbox has gone over its send quota", "Exceeding a quota produces different NDR codes and affects normal sending as well."),
                new Wrong("The domain's SPF record is failing", "SPF failures are reported by the receiving system and have nothing to do with a forwarding policy.")),
            new Step(
                "Which setting controls this behavior?",
                "The automatic forwarding setting in the outbound anti-spam policy in the Microsoft Defender portal",
                "Automatic forwarding rules are configured in the outbound spam filter policy, under Threat policies > Anti-spam.",
                new Wrong("The Conditional Access policy that applies to Marco", "Conditional Access evaluates sign-ins, not how Exchange routes mail."),
                new Wrong("The inbound anti-spam policy", "Inbound policies handle mail arriving in the tenant. Forwarding is an outbound control."),
                new Wrong("Marco's Outlook rules and alerts settings", "The rule itself is working. It's being blocked by tenant policy after it fires.")),
            new Step(
                "The business has approved this forwarding for Marco. How should you apply the change?",
                "Create a custom outbound policy with forwarding set to On and scope it to Marco",
                "A custom policy allows the exception for one approved user and leaves the secure default in place for everyone else.",
                new Wrong("Set automatic forwarding to On in the Default outbound policy, since that's the policy the mailbox is using today", "That permits external forwarding for all users, including any compromised account."),
                new Wrong("Give Marco the Global Administrator role", "Admin roles don't exempt a user from mail policy, and this grossly over-privileges the account."),
                new Wrong("Remove the outbound policy", "The Default policy can't be deleted, and removing protection wouldn't be the right fix anyway.")),
            new Step(
                "How do you confirm the fix?",
                "Send a test message that triggers the rule, then confirm delivery with a message trace",
                "Policy changes can take a little while to apply. A trace proves that the forwarded copy left the tenant and was delivered.",
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
                "The Exchange Online PowerShell V3 module signs in with modern authentication, supports MFA, and loads the Exchange Online cmdlets.",
                new Wrong("Connect-MsolService -Credential (Get-Credential)", "The MSOnline module is retired, and it never managed mailboxes or inbox rules."),
                new Wrong("New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell-liveid/ -Authentication Basic", "Basic authentication for remote PowerShell has been turned off in Exchange Online, so this connection fails."),
                new Wrong("Connect-MgGraph -Scopes 'Mail.ReadWrite', then run Get-InboxRule", "Microsoft Graph PowerShell doesn't load Exchange cmdlets, so Get-InboxRule isn't available in that session.")),
            new Step(
                "Trace already shows Delivered. Which command shows what happens to the mail inside the mailbox?",
                "Get-InboxRule -Mailbox grace@contoso.example -IncludeHidden | Format-List Name,Description",
                "Inbox rules act after delivery, and -IncludeHidden returns rules that Outlook doesn't display. The Description property spells out each rule's conditions and actions.",
                new Wrong("Get-MessageTraceV2 -RecipientAddress grace@contoso.example -StartDate (Get-Date).AddDays(-7) -EndDate (Get-Date)", "Trace records what transport did, and it already shows Delivered. It can't see what happened to a message after it reached the mailbox."),
                new Wrong("Get-MailboxStatistics -Identity grace@contoso.example", "This reports item counts and mailbox size, not why particular messages disappear."),
                new Wrong("Get-MailboxFolderPermission -Identity grace@contoso.example:\\Inbox", "Folder permissions show who can open the Inbox. They don't show rules that move mail out of it.")),
            new Step(
                "The output shows a hidden rule named '.' that moves messages containing 'bank' or 'wire' to RSS Subscriptions and marks them as read. What does this indicate?",
                "The mailbox is probably compromised, and an attacker created the rule to hide the bank's replies",
                "A one-character name, a hidden rule, and finance keywords moved to an unused folder are classic signs of business email compromise.",
                new Wrong("Grace created a cleanup rule by mistake and forgot about it", "Outlook doesn't create hidden rules, and the keywords target exactly the wire transfer conversation."),
                new Wrong("A Defender for Office 365 policy moved the messages as suspected phishing", "Defender actions show up in message trace and quarantine, not as an inbox rule in the user's mailbox."),
                new Wrong("An Outlook sync problem on Grace's laptop is filing new mail into RSS Subscriptions, so the messages land there automatically", "A client sync fault doesn't create a server-side rule with keyword conditions.")),
            new Step(
                "How do you contain this from PowerShell?",
                "Remove the rule, clear any mailbox forwarding with Set-Mailbox, reset the password, and run Revoke-MgUserSignInSession",
                "Removing the rule and forwarding stops the hiding, while the password reset and session revocation lock out the attacker so the rule can't be recreated.",
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
                "This wizard handles the account, the license assignment, and the per-app settings in one flow.",
                new Wrong("Exchange admin center > Recipients > Contacts > Add a mail contact", "A mail contact is an external address entry with no sign-in or license."),
                new Wrong("Teams admin center > Users", "This manages Teams settings for existing users. It doesn't create accounts."),
                new Wrong("Microsoft Purview > Roles & scopes", "Purview roles govern compliance permissions, not user provisioning.")),
            new Step(
                "The wizard reaches product licenses. What must be set before the E3 license can be assigned?",
                "The usage location for the user",
                "Licenses can't be assigned without a usage location because service availability varies by country or region.",
                new Wrong("A manager in Microsoft Entra ID", "A manager is useful directory data but it isn't a licensing requirement."),
                new Wrong("A completed MFA registration", "MFA is registered by the user later and doesn't block license assignment."),
                new Wrong("A department attribute", "Department is optional unless your own automation depends on it.")),
            new Step(
                "How do you keep Dana from getting SharePoint and Viva Engage while keeping the rest of E3?",
                "Select the E3 license, expand Apps, and turn off the SharePoint and Viva Engage service plans",
                "Disabling individual service plans within the license is the supported way to give a user a subset of a subscription.",
                new Wrong("Block Dana from SharePoint and Viva Engage with a Conditional Access policy, and leave the full E3 license in place", "Conditional Access blocks sign-ins to apps, but Dana would still be licensed and it doesn't change the entitlement HR asked about."),
                new Wrong("Assign Exchange Online Plan 1 instead of E3", "This loses the rest of E3, such as the Office apps, which Dana is meant to have."),
                new Wrong("Delete the SharePoint site collection for the Warehouse team", "This destroys content for others and doesn't affect Dana's entitlement.")),
            new Step(
                "How do you verify the result before Monday?",
                "Open Dana's user pane and check Licenses and apps to confirm that those two apps are off",
                "This view shows what's actually provisioned for the user, including each service plan's state.",
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
                "Membership drives the assignment, so joiners get the license and leavers release it without any admin effort.",
                new Wrong("Bulk-select all 50 users in Active users once", "That fixes today's list, but the next joiner has to be handled by hand again."),
                new Wrong("Add a mail flow rule for the Marketing department", "Mail flow rules process messages. They can't assign licenses."),
                new Wrong("Assign the license to a distribution list that HR keeps up to date for the Marketing department", "Group-based licensing needs a security-enabled group or Microsoft 365 group. A distribution list isn't supported.")),
            new Step(
                "When you assign E3 to the group, how do you exclude Viva Engage?",
                "Turn off the Viva Engage service plan in the group's license assignment options",
                "The group's assignment options apply the same service plan configuration to every member.",
                new Wrong("Add a second group with Viva Engage turned off, and put the same Marketing users in both groups", "Assignments are additive. A second group can't subtract what the first one grants."),
                new Wrong("Remove Viva Engage from each user afterward", "Manual per-user edits are the work you're trying to eliminate, and each joiner would need it too."),
                new Wrong("Disable Viva Engage for the tenant", "This affects every department, not just Marketing.")),
            new Step(
                "Three users show a license error. Where do you find out why?",
                "On the group's Licenses page in Microsoft Entra ID, review the users with assignment errors",
                "Entra ID records assignment errors per user in the group's licensing view, along with the reason for each.",
                new Wrong("In the Exchange message trace", "Message trace records mail flow, not licensing."),
                new Wrong("In the Sign-in logs for those users", "Sign-in logs show authentication attempts, not license processing."),
                new Wrong("In Microsoft Purview Audit, by searching for failed logons from those three users since the assignment ran", "Failed logons aren't related to a license assignment error.")),
            new Step(
                "The error says the users have no usage location. How do you fix it?",
                "Set the usage location on those users, then reprocess the assignment if it doesn't retry by itself",
                "Once the missing property is set, the group can assign the license. Reprocessing gets it done without waiting.",
                new Wrong("Remove them from the group permanently", "They still need the license, so this just abandons the requirement."),
                new Wrong("Purchase three additional E3 licenses", "The cause is a missing attribute, not a shortage of licenses."),
                new Wrong("Change the tenant's default language", "Language settings have no effect on usage location."))),

        new(
            "entra-ca-travel-block",
            ScenarioCategory.UserLicenseManagement,
            "Conditional Access blocks a traveling user",
            "Luis from Sales is at a client site in Lisbon and can't sign in to Outlook or Teams. The browser shows error AADSTS53003, 'Access has been blocked by Conditional Access policies'. No MFA prompt appears, and the same account worked from the office yesterday.",
            "Microsoft Entra ID evaluates every sign-in against the Conditional Access policies that apply to it, and the sign-in log records the result of each policy. AADSTS53003 means a policy's grant control was Block, which is why no MFA prompt appeared: a block is applied before any challenge is offered. Country-based named locations are resolved from the sign-in's public IP address, so a user abroad falls outside an 'allowed countries' location. The supported way to make an exception is an exclusion group with a documented end date, ideally governed by an access review. Disabling the policy, switching it to Report-only, or trusting a hotel IP weakens protection for everyone. The What If tool evaluates a simulated sign-in against every policy, so you can prove the exception works before the user tries again.",
            new Step(
                "Where do you find out why the sign-in was blocked?",
                "Open Luis's failed sign-in in the Entra sign-in logs and review its Conditional Access tab",
                "The sign-in log entry shows the error code, the location, and the result of every policy evaluated, including the one that blocked access.",
                new Wrong("Require Luis to re-register MFA from the Authentication methods page", "No MFA prompt was shown, so the methods were never used. Re-registering removes working methods without touching the block."),
                new Wrong("Check that Luis still has an E3 license assigned", "A missing license causes service errors after sign-in, not a Conditional Access block during it."),
                new Wrong("Search the Microsoft Purview audit log for Luis's mailbox activity since the last successful sign-in yesterday", "Mailbox auditing records actions inside the mailbox. It doesn't hold Conditional Access results.")),
            new Step(
                "The log shows error 53003, location Portugal, and the policy 'Block sign-ins outside allowed countries' with the result Failure. What is the cause?",
                "The policy blocked the sign-in because Portugal isn't in the allowed countries location",
                "Country locations come from the sign-in's public IP. Luis's IP maps to Portugal, which the policy blocks.",
                new Wrong("Luis failed the MFA challenge too many times and the account was locked out", "MFA failures return codes such as 500121, and a lockout returns 50053. The log shows a policy block."),
                new Wrong("Luis's password expired while traveling", "An expired password returns 50055 and prompts for a change. It isn't a Conditional Access result."),
                new Wrong("The hotel Wi-Fi is blocking traffic to login.microsoftonline.com, so the token request never completes", "The request reached Entra ID and was evaluated, because the sign-in log has an entry with a policy result.")),
            new Step(
                "Management approves access for the two-week trip. How do you apply it?",
                "Add Luis to a travel exclusion group that the policy excludes, with a set removal date",
                "The exception covers only the approved user for the approved period, and the policy keeps protecting everyone else.",
                new Wrong("Switch the policy to Report-only mode until Luis returns", "Report-only stops enforcement for every user in scope, not just Luis."),
                new Wrong("Add the hotel's public IP address to a trusted named location", "The IP can change, and trusting it opens the policy to anyone on that network, including attackers."),
                new Wrong("Add Portugal to the allowed countries location in the policy, then remove it when the trip ends", "That allows sign-ins from Portugal for the entire organization, and removing it later is easy to forget.")),
            new Step(
                "How do you confirm the exception before Luis tries again?",
                "Use What If for Luis with Portugal as the country, and check the block policy isn't applied",
                "What If simulates the sign-in against every policy, so it proves the exclusion works. Luis's next sign-in log entry then confirms it in production.",
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
                "It's the only option here that includes a shared mailbox, calendar, files, and a Planner board.",
                new Wrong("Distribution list", "A distribution list only sends email to its members, with no calendar, files, or tasks."),
                new Wrong("Mail-enabled security group", "This combines email delivery with access permissions but offers no collaboration workspace."),
                new Wrong("Dynamic distribution group", "Membership is automatic, but it's still email-only.")),
            new Step(
                "Where do you create it?",
                "Microsoft 365 admin center > Teams & groups > Active teams & groups, then add a Microsoft 365 group",
                "This is the standard admin path, and it provisions the connected group resources.",
                new Wrong("SharePoint admin center > Term store, then add a term set for the project team and assign its members to it", "The term store manages metadata and doesn't create groups."),
                new Wrong("Defender portal > Threat policies", "Threat policies configure security protection, not groups."),
                new Wrong("Exchange admin center > Mail flow > Rules", "Rules process messages and can't create a group.")),
            new Step(
                "What must you assign when creating the group?",
                "At least one owner, ideally two, who will manage its members",
                "Owners keep the group's membership and settings current without admin involvement, and a second owner guards against orphaning it.",
                new Wrong("A Global Administrator as the only member, so that membership changes stay with IT", "A group is meant to be run by its team, and this over-involves a privileged account."),
                new Wrong("A dedicated E5 license for the group", "A group doesn't need its own license."),
                new Wrong("A retention label on each member", "Retention labels apply to content, not to group membership.")),
            new Step(
                "Partners report that their emails to the group are being rejected. What should you change?",
                "Turn on the setting that allows people outside the organization to email the group",
                "By default, a Microsoft 365 Group only accepts mail from internal senders. This setting opens it to external senders.",
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
                "Only Send As hides the delegate completely. The recipient sees just the group's address.",
                new Wrong("Send on Behalf", "Recipients would see 'Aisha on behalf of Support', which breaks the requirement."),
                new Wrong("Full Access", "That's a mailbox permission for reading a mailbox. It doesn't apply to a distribution list or let you send as it."),
                new Wrong("Group owner", "Owners manage membership but get no right to send as the group.")),
            new Step(
                "Where do you grant it in the admin UI?",
                "Exchange admin center > Recipients > Groups > Distribution list > the group's Delegates settings",
                "Delegate permissions for a group, including Send As, are managed on the group in the Exchange admin center.",
                new Wrong("Microsoft 365 admin center > Roles > Role assignments, then assign Aisha the Exchange Recipient Administrator role", "Admin roles grant administrative rights, not sending rights on a single group."),
                new Wrong("Microsoft Entra ID > App registrations", "App registrations are for applications and have no connection to mail delegation."),
                new Wrong("Outlook on the web > Aisha's signature settings", "A signature changes the message text, not the sender's permissions.")),
            new Step(
                "You'd rather script this. Which PowerShell command is right?",
                "Add-RecipientPermission -Identity support@contoso.example -Trustee aisha@contoso.example -AccessRights SendAs",
                "In Exchange Online, Add-RecipientPermission is the cmdlet that grants Send As.",
                new Wrong("Add-MailboxPermission -Identity support@contoso.example -User aisha@contoso.example -AccessRights FullAccess -InheritanceType All", "This grants Full Access to a mailbox, and a distribution list doesn't have one."),
                new Wrong("Set-DistributionGroup -Identity support@contoso.example -GrantSendOnBehalfTo aisha@contoso.example", "This grants Send on Behalf, which reveals Aisha's name."),
                new Wrong("Add-DistributionGroupMember -Identity support@contoso.example -Member aisha@contoso.example", "Membership means Aisha receives the group's email, not that Aisha can send as it.")),
            new Step(
                "Aisha still can't send as the group right after you grant it. What's the most likely next step?",
                "Allow time for the permission to replicate, then have Aisha choose the support address in the From field",
                "Permissions can take a while to take effect, and Outlook sends from the user's own address unless the From field is changed.",
                new Wrong("Grant the Exchange Administrator role to Aisha", "An admin role is unnecessary and wouldn't fix replication timing."),
                new Wrong("Recreate the distribution list", "This disrupts existing members and addresses without addressing the cause."),
                new Wrong("Convert the distribution list to a shared mailbox immediately", "A migration isn't justified just because a permission hasn't propagated yet."))),

        new(
            "spo-external-sharing-domain",
            ScenarioCategory.GroupManagement,
            "External sharing blocked for one partner",
            "The Project Atlas team keeps its files on a Microsoft 365 Group site. Owners can share with partners at adatum.example, but sharing with a new consultant at fabrikam.example fails with 'Your organization's policies don't allow you to share with these users.' Organization-level external sharing is set to New and existing guests.",
            "SharePoint and OneDrive sharing works in layers. The organization-level setting in the SharePoint admin center is the most permissive level allowed, and each site can match it or be more restrictive. Domain restrictions apply on top of both, either for the whole organization under Policies > Sharing ('Limit external sharing by domain') or for a single site. Sharing with a domain that isn't on an allow list fails with this policy message, which is why one partner works and another doesn't. SharePoint also uses Microsoft Entra B2B for guests, so an allow or deny list in the Entra external collaboration settings can block the invitation too, and the two lists should agree. The narrow fix is adding the approved domain to the allow list, not loosening the sharing level or removing the restriction for everyone.",
            new Step(
                "The organization level already allows guests. Where do you look first?",
                "The Project Atlas site's sharing settings in the SharePoint admin center, then any domain limits",
                "Below the organization level, the site setting and domain restrictions are the layers that can block one partner but not another.",
                new Wrong("The consultant's guest account in Entra ID, to see whether sign-in is blocked", "The invitation failed before any guest account was created, so there's no account to check."),
                new Wrong("The guest access settings in the Teams admin center", "Teams guest access controls the guest experience in Teams, not sharing from SharePoint."),
                new Wrong("The data loss prevention policy matches in Microsoft Purview for the files the owners tried to share with the consultant", "A DLP block shows a policy tip about sensitive content, and it would affect adatum.example as well.")),
            new Step(
                "The site allows New and existing guests and has no domain list. Under Policies > Sharing, the organization limits sharing to allowed domains, and only adatum.example is listed. What's the cause?",
                "fabrikam.example isn't on the organization's allowed domains list",
                "An allow list blocks every domain that isn't on it, which is why adatum.example works and fabrikam.example doesn't.",
                new Wrong("The site's sharing level is more restrictive than the organization's", "You just confirmed the site allows New and existing guests."),
                new Wrong("The consultant's Microsoft account hasn't been verified yet", "Verification happens after an invitation is sent. This one was blocked before it was sent."),
                new Wrong("Fabrikam's own tenant has a cross-tenant access policy that blocks B2B collaboration with outside organizations", "A block in the partner's tenant fails at redemption, not with your organization's sharing policy message.")),
            new Step(
                "Security approves Fabrikam as a partner. What do you change?",
                "Add fabrikam.example to the allowed domains, and to the Entra B2B allow list if one is used",
                "This admits exactly the approved partner, and keeping the Entra list in step prevents a second block when the guest is invited.",
                new Wrong("Change the organization's sharing level to Anyone so links work without sign-in", "Anyone links remove identity checks for all content, and they still wouldn't satisfy the domain restriction."),
                new Wrong("Turn off 'Limit external sharing by domain' at the organization level so the consultant can be invited right away", "That opens sharing to every external domain to admit one partner."),
                new Wrong("Create an internal account for the consultant and assign it an E3 license", "That costs a license, bypasses guest governance, and gives a non-employee a full internal identity.")),
            new Step(
                "How do you confirm the fix?",
                "Have an owner share again, then confirm the consultant can redeem the invitation and open the file",
                "Only the full path proves it works, from invitation to sign-in to file access. Sharing changes can take a few minutes to apply.",
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
                "Guest blocks come either from tenant-wide settings or from one team's label or group setting. One test tells you which layer to inspect.",
                new Wrong("Ask the partner to create a Microsoft account first", "Guests can redeem with a work account, a Microsoft account or a one-time passcode, so the partner's account isn't what's blocking Mei."),
                new Wrong("Reinstall the Teams client on Mei's computer", "The search result reflects policy, not a client fault, and reinstalling removes nothing that blocks guests."),
                new Wrong("Turn guest access off and on again in the Teams admin center, then wait for the change to replicate across the tenant before trying again", "Toggling a setting that's already on changes nothing, and it disrupts existing guests.")),
            new Step(
                "Other teams can add the guest. Vendor Onboarding has the sensitivity label 'Confidential - Internal'. What do you check?",
                "Whether that label's group settings stop owners from adding guests",
                "Container sensitivity labels can control guest access for a Microsoft 365 group, and they override what the owner can do in that team.",
                new Wrong("Whether the label applies encryption to the team's files", "Encryption affects who can open labeled files, not who can be added as a member."),
                new Wrong("Whether Mei has a Teams Premium license", "Adding guests to a team doesn't need Teams Premium."),
                new Wrong("Whether the Entra external collaboration settings restrict guest invitations to admin roles", "That setting is tenant-wide, but other teams can already add the guest.")),
            new Step(
                "The label blocks guests by design, but the business needs the partner in this team. What is the right fix?",
                "Get the data owner's approval for a label that allows guests, then relabel the team",
                "The label records a classification decision, so the owner of the data must approve the change instead of IT working around it.",
                new Wrong("Remove the sensitivity label from the team so the default settings apply", "This drops the label's other protections and bypasses the data owner's decision."),
                new Wrong("Create a member account for the partner in your tenant so the label's guest restriction doesn't apply", "This sidesteps the classification and gives an outside person an internal identity and license."),
                new Wrong("Assign Mei the Teams Administrator role", "Admin roles don't override a label's guest setting, and the role grants far too much access.")),
            new Step(
                "The guest accepted the invitation but says the team doesn't show up in Teams. What do you tell them?",
                "Switch to the Contoso organization in Teams, using the account switcher on the profile picture",
                "Guest membership lives in the host tenant, and Teams only shows that tenant's teams after the guest switches to it.",
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
                "Mail flow rules live in the Exchange admin center and apply at the transport level, so they cover every client.",
                new Wrong("An Outlook inbox rule on each mailbox, deployed with a script", "Client rules are per mailbox, can be changed by users, and can't insert a disclaimer."),
                new Wrong("A Defender anti-phishing policy", "Anti-phishing policies apply protection and warnings, but they can't add custom disclaimer text."),
                new Wrong("A Microsoft Purview retention policy", "Retention policies govern how long content is kept, not its message text.")),
            new Step(
                "Which condition targets the right messages?",
                "The sender is located > Outside the organization",
                "This matches mail whose sender is external to the tenant, which is exactly what Security requested.",
                new Wrong("The recipient is located > Outside the organization", "That matches outbound mail, the opposite of what's needed."),
                new Wrong("The subject or body includes 'external'", "That depends on message content and misses nearly all external mail."),
                new Wrong("Apply to all messages", "Internal mail would be stamped as external too, and people would soon learn to ignore the warning.")),
            new Step(
                "Which action adds the warning to the bottom of each message, and what should happen if it can't be inserted?",
                "Apply a disclaimer > append a disclaimer, with the fallback action set to Wrap",
                "Append places the text at the end of the message, and Wrap makes sure that a message that can't be modified still carries the warning in an outer message.",
                new Wrong("Prepend the subject with 'EXTERNAL' and no disclaimer", "Changing the subject adds a tag, but it doesn't add the required warning text to the bottom of the message."),
                new Wrong("Apply a disclaimer > prepend a disclaimer, with the fallback action set to Reject", "Prepend puts the text at the top, and Reject would bounce legitimate encrypted mail."),
                new Wrong("Redirect the message to a Security mailbox", "That diverts mail away from the recipients instead of warning them.")),
            new Step(
                "The rule is saved. How do you roll it out safely?",
                "Run it in test mode, review the results, then switch to Enforce and enable the rule",
                "Test mode shows how the rule matches without changing live mail. New rules can also start disabled, so check that it's enabled.",
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
                "Trace details record the mail flow rules that were applied to the message, so you can see what ran.",
                new Wrong("Delete the rule and recreate it from scratch", "It works for other mail, so the rule itself isn't broken, and you'd lose the chance to find the cause."),
                new Wrong("Check the vendor's SPF record", "SPF affects sender authentication. It wouldn't selectively skip a transport rule."),
                new Wrong("Check the affected users' Outlook view settings, in case the disclaimer is collapsed or hidden in the reading pane", "This happens to every recipient of that vendor's mail, so individual client settings aren't the cause.")),
            new Step(
                "The trace shows that a rule named 'Vendor Bypass' matched and your disclaimer rule didn't. What do you check?",
                "The priority order of the two rules, and whether 'Vendor Bypass' has 'Stop processing more rules' turned on",
                "A higher-priority rule with this option on prevents later rules from running for messages it matches.",
                new Wrong("Whether the vendor's domain has expired", "An expired domain wouldn't explain the mail arriving and matching a rule."),
                new Wrong("Whether the disclaimer text is over a length limit, or contains HTML that the rule can't insert into the vendor's messages", "That wouldn't produce a failure for just one sender."),
                new Wrong("The mailbox size of the recipients", "Mailbox size has no effect on which rules are evaluated.")),
            new Step(
                "That's the cause. What is the cleanest fix?",
                "Move the disclaimer rule above 'Vendor Bypass', or turn off 'Stop processing more rules' if it's not needed",
                "Either change lets the disclaimer rule run while keeping the vendor rule's other behavior.",
                new Wrong("Delete 'Vendor Bypass' without asking why it exists", "Someone created it for a reason, so removing it may break something else."),
                new Wrong("Add a second disclaimer rule at the bottom of the list", "It sits below the stop rule as well, so it would be skipped in the same way."),
                new Wrong("Add a disclaimer action to every other rule", "This duplicates logic and makes the rule set harder to maintain.")),
            new Step(
                "How do you confirm the change worked?",
                "Have a vendor message come through, then confirm the disclaimer and check the trace details",
                "A real message and its trace prove the expected rule fired. Rule changes can take a short time to apply.",
                new Wrong("Trust that the new order is correct and close the ticket", "This skips verification, and rule changes may take time to take effect."),
                new Wrong("Send an internal test message", "An internal message doesn't meet the external sender condition, so the rule wouldn't match."),
                new Wrong("Check the Exchange admin center service health page only", "Service health reports Microsoft outages, not whether your rule fired."))),

        new(
            "defender-phish-campaign",
            ScenarioCategory.MailFlowRules,
            "Responding to a credential phishing campaign",
            "At 10:40, three users report an email titled 'Payroll update required' that links to a fake Microsoft sign-in page. It came from hr-notice@payroll-portal.example and passed SPF and DKIM for that domain. The tenant has Microsoft Defender for Office 365 Plan 2.",
            "Threat Explorer in Defender for Office 365 Plan 2 finds every copy of a campaign by sender, subject or URL, and shows each copy's delivery location along with URL clicks recorded by Safe Links. You can act on the results directly, for example with a soft delete, and the Action center tracks the remediation. Zero-hour auto purge (ZAP) can also remove delivered messages when a verdict changes, but you can't rely on its timing during an active campaign. A sender that passes SPF and DKIM for its own lookalike domain isn't spoofing you, so your own DMARC policy won't stop it, and the Tenant Allow/Block List is the place to block the domain and the URL. Anyone who clicked through to the credential page may have given away a password, so reset it, revoke their sessions and review their sign-ins.",
            new Step(
                "How do you find everyone who received the message?",
                "Search Threat Explorer in the Defender portal by sender address and subject for today",
                "Explorer returns every matching message with its recipients, delivery location and URL clicks in one view.",
                new Wrong("Ask the three reporters to forward the message to the service desk", "Their copies don't tell you who else received it."),
                new Wrong("Run a mail flow rule report for messages from payroll-portal.example", "Rule reports show which rules matched, not a full list of recipients."),
                new Wrong("Check the Service health page in the Microsoft 365 admin center for an active phishing advisory affecting the tenant", "Service health reports Microsoft service issues, not campaigns aimed at your users.")),
            new Step(
                "Explorer shows 140 recipients, with 128 copies still in Inboxes. How do you remove them?",
                "Select the messages in Explorer, take a Soft delete action, and track it in the Action center",
                "Remediating from Explorer pulls delivered copies out of every mailbox at once, and the Action center records the result.",
                new Wrong("Create a mail flow rule that deletes messages from payroll-portal.example", "Mail flow rules act on messages in transit, so they can't touch copies that were already delivered."),
                new Wrong("Wait for zero-hour auto purge to remove them", "ZAP only acts when a verdict changes, and you can't count on its timing while users are clicking."),
                new Wrong("Email all staff a warning, then ask each recipient to delete the message and empty Deleted Items", "People miss warnings, and some will click before they read it.")),
            new Step(
                "How do you block further messages and the link itself?",
                "Add block entries for the domain and URL in the Tenant Allow/Block List",
                "Block entries stop new mail from the domain, and Safe Links blocks the URL at click time even in messages that get through.",
                new Wrong("Publish a DMARC policy of p=reject for your own domain", "The attacker sends from its own domain and passes its own SPF and DKIM. Your DMARC policy only protects your domain."),
                new Wrong("Block the sending IP address in the connection filter policy", "Campaigns rotate sending IPs, and an IP block does nothing about the URL in other messages."),
                new Wrong("Add payroll-portal.example to the protected domains for impersonation in the anti-phishing policy", "Impersonation protection guards domains you own or care about. The attacker's domain isn't impersonating anything.")),
            new Step(
                "Explorer's URL clicks show that two users clicked through the Safe Links warning. What do you do next?",
                "Treat both accounts as compromised: reset passwords, revoke sessions, and review sign-ins",
                "A click through to a credential page may have captured a password, and revoking sessions cuts off any tokens the attacker already has.",
                new Wrong("Run a full antivirus scan on both users' computers", "Credential phishing steals passwords in the browser, and a malware scan doesn't undo that."),
                new Wrong("Assign both users a phishing awareness training module", "Training is worthwhile later, but it doesn't contain a possible compromise now."),
                new Wrong("Move both users into the Strict preset security policy so future phishing messages are quarantined before they reach the Inbox", "That protects against the next message, not against credentials that may already be stolen."))),

        // ------------------------------------------------------------------- VOIP support
        new(
            "voip-one-way-audio",
            ScenarioCategory.VoipSupport,
            "One-way audio at a branch office",
            "Every desk phone at a branch office can place and answer calls through the company's hosted PBX, but on all calls the staff hear nothing while the people they call hear them clearly. Phones show as registered, and the problem began after the branch firewall was replaced.",
            "SIP sets up a call, and RTP carries the audio as a separate UDP stream, so a call can connect perfectly with audio failing in one direction. Each side uses the SDP body in the SIP messages to announce the IP address and port where it wants to receive RTP. When a phone behind NAT advertises its private address, the far end sends audio to an address it can't route to. This is the classic one-way audio pattern, and it fits here because the new firewall changed how NAT was handled. Fix it with proper NAT traversal, such as STUN, or an SBC or PBX that uses symmetric RTP. Also disable SIP ALG if the firewall's rewriting is making things worse, and make sure the RTP UDP port range is allowed. Compare captures from both sides of the firewall, because they show whether a stream was never sent or was lost on the way.",
            new Step(
                "Calls connect normally, but audio only flows one way. What does that tell you?",
                "SIP signaling is working, so the fault is in the RTP media path",
                "A call that rings and connects has completed its SIP exchange. Audio travels in a separate RTP stream that can fail on its own.",
                new Wrong("The phones and the hosted PBX have a codec mismatch, so audio can't be decoded on one side", "With no common codec the call normally fails during setup, often with 488 Not Acceptable Here. It wouldn't connect and carry one good audio direction."),
                new Wrong("SIP registration is failing intermittently at the branch", "The phones show as registered and can make and receive calls, so registration isn't the problem."),
                new Wrong("The handsets' earpieces need to be replaced at the branch", "Every phone at the site failing at once, right after a firewall change, points to the network and not to hardware.")),
            new Step(
                "How do you get evidence of what is happening to the audio?",
                "Take a packet capture during a test call and inspect the SIP and RTP streams in Wireshark",
                "A capture shows the SDP each side advertised and which RTP streams actually exist. Wireshark's Telephony > VoIP Calls view lays this out for each call.",
                new Wrong("Run a sustained ping from a phone's VLAN to the hosted PBX address", "Ping proves basic reachability with ICMP. It says nothing about which addresses and ports RTP is using."),
                new Wrong("Review the call detail records in the hosted PBX portal to see which direction of each call failed", "CDRs show that the call connected and how long it lasted. They don't show which way the media was flowing."),
                new Wrong("Run an internet speed test from a PC on the branch network", "Bandwidth shortage causes poor quality on both directions, not a clean loss of audio in one.")),
            new Step(
                "In the capture, the phone's SDP says c=IN IP4 192.168.10.25. RTP from the phone leaves the site, but no RTP comes back. What is the cause?",
                "The phone is advertising a private IP, so the far end sends RTP to an unroutable address",
                "The SDP c= line tells the other side where to send audio. A 192.168.x.x address means nothing on the internet, so the return stream never arrives.",
                new Wrong("DSCP markings are missing from the voice traffic, so the ISP is dropping the return RTP packets", "Missing QoS markings lower priority under congestion. They don't remove one direction of audio completely."),
                new Wrong("The jitter buffer on the phones is too small for this WAN link", "A small jitter buffer causes choppy audio from late packets. Here no return packets arrive at all."),
                new Wrong("The SRTP keys don't match, so the phone is discarding the audio", "A key mismatch would show RTP packets arriving but undecodable. This capture shows no return stream at all.")),
            new Step(
                "What is the right fix?",
                "Configure NAT traversal so SDP carries a reachable address, and allow the RTP port range",
                "STUN, or an SBC or PBX using symmetric RTP, gets audio back through NAT. The firewall must also pass the provider's RTP UDP range.",
                new Wrong("Open TCP port 5060 inbound from the hosted PBX to the phones so their audio can reach the branch", "Port 5060 carries SIP signaling, which already works. RTP uses a range of UDP ports."),
                new Wrong("Force every phone to use the G.711 codec instead of G.729", "Changing the codec changes how audio is encoded, not the unreachable address in the SDP."),
                new Wrong("Shorten the SIP registration interval on all branch phones", "Registration keeps signaling reachable. It doesn't change where the far end sends RTP.")),
            new Step(
                "How do you confirm the fix?",
                "Test calls in both directions, confirming two-way audio and two RTP streams in a capture",
                "Testing inbound and outbound calls covers both call paths, and the capture proves media flows each way instead of relying on one good impression.",
                new Wrong("Confirm that all of the phones still show as registered", "They were registered during the fault too, so this proves nothing about audio."),
                new Wrong("Check that the firewall rule's hit counter is going up on the new RTP rule during several live calls", "A rising counter shows traffic matched the rule, not that a person can hear audio in each direction."),
                new Wrong("Place a single internal extension-to-extension test call", "Internal calls may stay on the local network and never cross the NAT path that was failing."))),

        new(
            "voip-sip-408-timeout",
            ScenarioCategory.VoipSupport,
            "SIP 408 Request Timeout on a remote phone",
            "Calls to a remote worker's extension fail after about 32 seconds, and the caller's phone shows 408 Request Timeout. The phone sits behind a home router and shows as registered. Oddly, it's reachable for a minute or two right after it's rebooted, and calls made from that phone always work.",
            "A SIP 408 means a request got no final response within the allowed time. For an INVITE over UDP, Timer B expires at 64 times T1, which is 32 seconds with the default 500 ms T1, and that matches this ticket. Retransmissions with no provisional response show the INVITE isn't reaching the phone, or the phone's reply isn't getting back. Outbound calls work because the phone opens its own path through the router. Inbound calls depend on a NAT or firewall UDP mapping staying open. When the mapping expires between registrations, the PBX's registration is still valid, but its INVITE is dropped at the router. Keep-alives sent more often than the router's UDP timeout hold the mapping open. Avoid changing SIP timers, because a longer wait just delays the same failure.",
            new Step(
                "What does SIP 408 Request Timeout tell you?",
                "No final response to the request arrived in time, so find where the request stopped",
                "408 is about time running out. The request got no final answer, so the next job is to find which hop it stopped at.",
                new Wrong("The called user pressed decline on the handset", "A deliberate decline usually returns 603 Decline, or 486 Busy Here, not a timeout."),
                new Wrong("The PBX searched its dial plan, found no such extension, and rejected the call with a timeout", "An unknown destination returns 404 Not Found, usually straight away rather than after 32 seconds."),
                new Wrong("The caller failed digest authentication on the PBX", "Authentication challenges return 401 or 407, and they arrive immediately.")),
            new Step(
                "How do you see where the call is failing?",
                "Capture the call's SIP signaling on the PBX with sngrep or tcpdump and read the ladder",
                "A SIP ladder shows each request and response in order, so you can see exactly what the PBX sent and what it got back.",
                new Wrong("Reboot the PBX during a quiet period and retest the call", "This destroys the evidence, affects every user, and doesn't explain a single phone's failure."),
                new Wrong("Check the RTP packet statistics for the failed call on the PBX to see where audio stopped flowing", "No media is set up for a call that never connected, so there are no RTP statistics to read."),
                new Wrong("Replace the remote worker's handset with a spare phone", "Outbound calls work, so the phone isn't obviously broken, and swapping it skips the diagnosis.")),
            new Step(
                "The ladder shows the PBX resending the INVITE to the phone's registered public address at 0.5, 1, 2 and 4 seconds, with no 100 Trying or 180 Ringing. What does this mean?",
                "The INVITE isn't reaching the phone, or its replies aren't returning, until Timer B expires",
                "Retransmission means nothing came back. A phone that received the INVITE would answer with a provisional response, and 32 seconds matches Timer B.",
                new Wrong("The phone is ringing, but nobody is there to answer it", "A ringing phone sends 180 Ringing, and that never appears in this ladder."),
                new Wrong("The PBX is overloaded and is shedding incoming calls, so it's resending each INVITE until one gets through", "An overloaded server normally answers 503 Service Unavailable, and other extensions work fine."),
                new Wrong("The two ends have no codec in common for this call", "A codec failure comes back as a response such as 488. Here there's no response of any kind.")),
            new Step(
                "The phone last registered 40 minutes ago, and the home router's UDP timeout is 60 seconds. What is the best fix?",
                "Enable SIP keep-alives, sent more often than the router's 60-second UDP timeout",
                "CRLF or OPTIONS keep-alives refresh the NAT mapping, so inbound INVITEs have an open path. This also explains why rebooting helps for a short time.",
                new Wrong("Raise Timer B on the PBX from 32 seconds to 120 seconds so the phone has more time to respond", "The INVITE is being dropped at the router, so waiting longer just makes the same failure slower."),
                new Wrong("Move the extension to a different number on the PBX", "The number isn't the problem. The new extension would hit the same expired mapping."),
                new Wrong("Port-forward all UDP ports on the home router to the phone", "This exposes the phone to the internet and is hard to maintain, when a keep-alive solves the actual cause.")),
            new Step(
                "How do you prove the fix works?",
                "Wait well past the 60-second UDP timeout, then call in and confirm 100 Trying and 180 Ringing",
                "The fault only appears once the mapping has aged out, so the test has to wait for that. A ladder with provisional responses confirms the path stayed open.",
                new Wrong("Call the phone straight after you've rebooted it", "This phone already worked right after a reboot, so that test would pass even without a fix."),
                new Wrong("Have the remote worker place an outbound test call", "Outbound calls always worked. The problem is only with inbound calls."),
                new Wrong("Confirm that the phone is displaying a green registered icon", "It showed as registered during the failures too, so the icon proves nothing."))),

        // -------------------------------------------------------- Network troubleshooting
        new(
            "network-packet-loss-mtr",
            ScenarioCategory.NetworkTroubleshooting,
            "Diagnosing packet loss with ping and MTR",
            "Users at a branch office report choppy video calls and stalls in a cloud app at 203.0.113.50, starting this morning. Everything on the local file server works normally. You're on a Linux machine on the branch LAN.",
            "Ping is the quickest way to scope a problem: test the default gateway first, then an external target. MTR then combines ping and traceroute by sending a stream of probes to every hop. The rule for reading it is that loss only matters if it continues through to the final destination. A router in the middle showing loss when the hops after it don't is usually just rate limiting the ICMP or TTL-expired replies it generates, and it's forwarding traffic fine. Real loss starts at one hop and carries through to the end, which points to that hop or the link leading to it. Paths are often asymmetric, so run MTR from the far side too if you can, and check interface errors and utilization on your own equipment before escalating. Give the ISP timestamped MTR reports from both directions.",
            new Step(
                "Where do you start?",
                "Ping the default gateway, then an external IP, each with about 100 packets",
                "This quickly separates a LAN problem from a WAN problem. A larger count catches intermittent loss that a few packets would miss.",
                new Wrong("Ping 203.0.113.50 with the default four packets, since that's the address users are complaining about", "Four packets can't measure intermittent loss, and one target doesn't tell you where the loss is."),
                new Wrong("Run nslookup for the cloud app's hostname from a client", "Name resolution isn't what's failing. Users connect, but the connection is unstable."),
                new Wrong("Restart the branch core switch to clear out its counters", "This causes an outage, wipes evidence, and the local file server is already working fine.")),
            new Step(
                "The gateway shows 0% loss, and the external IP shows 5%. Which command shows where the loss begins?",
                "mtr -rwc 200 203.0.113.50",
                "Report mode with 200 probes per hop gives a statistically useful picture of loss at every hop on the path.",
                new Wrong("traceroute -n -m 30 203.0.113.50", "Traceroute sends only three probes per hop, which is too few to measure intermittent loss."),
                new Wrong("arp -a", "The ARP table shows local IP-to-MAC mappings and tells you nothing beyond the LAN."),
                new Wrong("netstat -an", "This lists local sockets and their state, not the loss along a network path.")),
            new Step(
                "MTR shows hop 5 at 40% loss, but hops 6 to 9, including the destination, show 0%. What do you conclude?",
                "Ignore hop 5, since loss that doesn't carry on is likely ICMP rate limiting",
                "If hop 5 were really dropping traffic, later hops would show loss too. It's just giving low priority to replies aimed at itself.",
                new Wrong("Hop 5 is dropping real traffic, so open a priority ticket with the network that owns that router", "Later hops prove traffic is passing through hop 5 cleanly, so that ticket would be rejected."),
                new Wrong("Add every hop's figure together to get a 40% end-to-end loss", "Loss isn't additive across hops, and the destination itself shows 0%."),
                new Wrong("The destination is hiding its true loss by blocking ICMP", "A host blocking ICMP would show 100% loss. It wouldn't show a clean 0%.")),
            new Step(
                "A later MTR shows 0% at hops 1 and 2, then 6% at hop 3, and about 6% on every hop through to the destination. Hop 2 is your firewall, and hop 3 is the ISP's gateway. What does this show?",
                "Real loss begins at hop 3 or the link into it, which is your ISP handoff",
                "Loss that starts at a hop and continues to the destination is genuine. Here that places it at the firewall-to-ISP link or the ISP's gateway.",
                new Wrong("The destination server is overloaded and dropping requests", "The server is only where the loss ends up. It clearly starts several hops earlier."),
                new Wrong("The final router before the destination is at fault because it's the last hop to show any loss", "Each later hop just inherits loss from upstream, so read from where it starts."),
                new Wrong("The LAN switch is dropping frames between clients and the firewall", "Hops 1 and 2 show 0%, so traffic is getting through the LAN and the firewall cleanly.")),
            new Step(
                "What should you do before contacting the ISP?",
                "Check the WAN interface for errors and saturation, then run MTR from the far side too",
                "Interface errors, a duplex mismatch or a full link would mean the fault is yours. Return paths can differ, so a reverse MTR gives the ISP the full picture.",
                new Wrong("Disable ICMP on the firewall so the MTR reports look cleaner", "That hides evidence and wouldn't change any real packet loss."),
                new Wrong("Raise the WAN interface MTU to 9000 to improve throughput", "Jumbo frames generally don't work across an ISP link, and this could add new faults."),
                new Wrong("Have users switch to a mobile hotspot until the loss clears up", "A workaround may help a single user, but it doesn't diagnose anything or restore the site.")),
            new Step(
                "The firewall's interface is clean and under 40% utilized. What is the right escalation?",
                "Open an ISP ticket with timestamped MTR reports from both directions",
                "This gives the ISP proof that the loss starts at its handoff and that your side has been ruled out, which shortens the investigation.",
                new Wrong("Tell the ISP the internet is slow and request a callback", "With no evidence, the ISP is likely to start from the beginning or close the ticket."),
                new Wrong("Replace the firewall, since it's the last device you control before traffic reaches the ISP", "Its interface is clean, so this is expensive and doesn't address the evidence."),
                new Wrong("Open a ticket with the cloud app vendor about its server", "The loss begins well before the vendor's network, so the vendor can't fix it."))),

        new(
            "network-dhcp-scope-exhaustion",
            ScenarioCategory.NetworkTroubleshooting,
            "Resolving DHCP scope exhaustion",
            "At 9 AM, new devices on the staff and guest VLAN 20 (192.168.20.0/24) can't get online and show 169.254.x.x addresses. Devices that were already connected still work. The VLAN uses a Windows DHCP server, and a conference has been running at the office all week.",
            "A 169.254.x.x address is an APIPA link-local address, which a client assigns itself when no DHCP server offers a lease. Devices that already hold a lease keep working, which is the classic sign of exhaustion rather than a server outage. A /24 has only 254 usable addresses, and a Windows DHCP scope defaults to an 8-day lease, so visitors' phones keep addresses long after leaving. Phones using private (randomized) MAC addresses can make this worse, since one device can look like several clients. Shorten the lease on busy guest networks, and reclaim stale leases. Changing the duration only affects new leases and renewals, so it needs some time or lease cleanup. Turn on conflict detection while reclaiming, since a device might still be using a removed address. Then fix capacity properly by widening the subnet or moving guests to their own VLAN.",
            new Step(
                "What does a 169.254.x.x address tell you?",
                "The client got no DHCP offer and assigned itself a link-local APIPA address",
                "Clients use APIPA when DHCP fails. It's a useful clue, since it means the problem is getting a lease, not routing or DNS.",
                new Wrong("A DNS server has handed out an address from the wrong zone", "DNS resolves names and has no part in assigning a client's IP address."),
                new Wrong("The client was given a reserved address by the DHCP server because its MAC is in the exclusion list", "A DHCP reservation would give an address from the scope, not a 169.254.x.x address."),
                new Wrong("The default gateway is performing NAT for the guest VLAN", "NAT doesn't assign addresses to clients, and 169.254.x.x isn't a NAT range.")),
            new Step(
                "Existing devices work, but new ones don't. What do you check first?",
                "The VLAN 20 scope statistics on the DHCP server, with Get-DhcpServerv4ScopeStatistics",
                "Existing clients working suggests the server is reachable. Scope statistics show right away whether the pool has run out of addresses.",
                new Wrong("Run ipconfig /renew on each affected device in turn", "Renewing asks for a lease again, and it fails the same way if the pool has none left."),
                new Wrong("Restart the DHCP Server service with Restart-Service DHCPServer to clear out the address pool", "Leases persist across a restart, so this interrupts service and frees nothing."),
                new Wrong("Replace the wireless access points serving the floor", "Wired and wireless clients on the VLAN share the same symptom, and existing clients work.")),
            new Step(
                "The scope shows 100% in use. The lease list is full of phones from the conference, with 8-day leases and many private MAC addresses. What is the root cause?",
                "Short-stay devices are holding addresses for 8 days, draining a small /24 scope",
                "Visitors' devices keep their addresses long after they've left, so the pool fills up with leases nobody is really using.",
                new Wrong("A rogue DHCP server on the VLAN is handing out bad addresses and taking leases from the real server", "Rogue DHCP gives clients a wrong address. It doesn't leave them with no offer at all."),
                new Wrong("The DHCP server's database has become corrupted", "The lease list is readable and accurate, and it shows real demand."),
                new Wrong("The gateway's ARP cache is full and is blocking new devices", "ARP is separate from DHCP, and the scope statistics already explain the failure.")),
            new Step(
                "What is the best immediate fix?",
                "Shorten the lease to a few hours, reclaim stale leases, and turn on conflict detection",
                "A shorter lease stops this from recurring, and removing stale leases frees addresses now. Conflict detection guards against an address still being in use.",
                new Wrong("Extend the lease duration to 30 days to stop clients renewing", "Longer leases hold addresses for even longer, which makes exhaustion worse."),
                new Wrong("Delete the scope and recreate it from scratch during the day so that every lease is released at once", "This discards the lease records and risks duplicate addresses with clients still using them."),
                new Wrong("Assign static IP addresses to each device that can't connect", "Static addresses don't scale for visitors, and they would collide with addresses the scope hands out.")),
            new Step(
                "Service is back. How do you prevent it from happening again?",
                "Move guests to a separate VLAN, or widen the subnet to a /23 during a change window",
                "Both add real capacity. A /23 means changing the gateway mask and the scope together, while a guest VLAN also keeps visitors away from staff devices.",
                new Wrong("Add a second scope in another subnet with no gateway address for it", "With no router interface in that subnet, those clients couldn't reach anything."),
                new Wrong("Set a scope threshold alert and make no other changes", "Alerts are worth adding, but they only warn you. They don't add a single address."),
                new Wrong("Turn off the private MAC address feature on visitors' phones", "You don't control visitors' devices, and the pool would still be too small."))),

        // ------------------------------------------------------------- Mac network issues
        new(
            "mac-dns-cache-mdnsresponder",
            ScenarioCategory.MacNetworkIssues,
            "Stale DNS cache on macOS",
            "Your team moved intranet.corp.example to a new server this morning and updated DNS. Windows laptops reach the new site, but a MacBook still loads the old server. The Mac has a normal internet connection, and browsing elsewhere works fine.",
            "macOS caches DNS results in the mDNSResponder daemon, and most apps resolve names through that system resolver. Tools such as dig and nslookup query DNS servers directly and skip the system resolver, while dscacheutil -q host follows the path that apps use. That makes comparing them a good test: if dig shows the new address and dscacheutil shows the old one, the fault is on the Mac rather than in DNS. Clear it with sudo dscacheutil -flushcache followed by sudo killall -HUP mDNSResponder. The HUP signal tells the daemon to drop its cache without stopping it. If the old answer survives a flush, look at what the system resolver checks besides DNS, such as an entry in /etc/hosts, or a scoped resolver from a VPN, which scutil --dns will list.",
            new Step(
                "How do you tell whether this is a local cache or a real DNS problem?",
                "Compare dig intranet.corp.example with dscacheutil -q host -a name intranet.corp.example",
                "dig asks the DNS server directly, while dscacheutil goes through the macOS resolver that apps use. A difference between them points at the Mac.",
                new Wrong("Run ipconfig /flushdns in Terminal and check the site again", "That's a Windows command. On macOS, ipconfig is a different tool and has no such option."),
                new Wrong("Run nslookup intranet.corp.example on the Mac, since it follows the same resolver path that apps use", "nslookup queries the DNS server directly, so it can't show you what the Mac's cache is holding."),
                new Wrong("Run ifconfig en0 to look at the Mac's interface details", "This shows the interface's addressing. It tells you nothing about how a name resolves.")),
            new Step(
                "dig returns the new IP, and dscacheutil returns the old one. What does that show?",
                "DNS is correct, and the Mac's system resolver is serving a stale answer",
                "The DNS server gave the right answer directly. Only the local path that apps use is still wrong, so the fault is on the Mac.",
                new Wrong("The authoritative DNS record hasn't been updated yet", "dig already got the new address, and Windows laptops are resolving it too."),
                new Wrong("The office router is intercepting DNS and rewriting responses for this one host on the Mac's subnet", "A rewriting router would affect dig as well, and Windows laptops on it work."),
                new Wrong("The web server is redirecting the Mac back to the old host", "dscacheutil shows the wrong IP before any web traffic happens.")),
            new Step(
                "Which command clears the DNS cache on macOS?",
                "sudo dscacheutil -flushcache; sudo killall -HUP mDNSResponder",
                "This flushes the directory service cache, then signals mDNSResponder to drop its cached DNS results.",
                new Wrong("sudo resolvectl flush-caches", "That's for systemd-resolved on Linux, which macOS doesn't use."),
                new Wrong("sudo launchctl disable system/com.apple.Finder; sudo killall Finder", "Finder isn't involved in DNS, and disabling system services can cause other problems."),
                new Wrong("sudo killall -9 networksetup", "networksetup is a command-line tool, not a running cache daemon.")),
            new Step(
                "After the flush, dscacheutil still returns the old IP, even though dig returns the new one. What do you check next?",
                "Look for an entry in /etc/hosts, then review the resolvers with scutil --dns",
                "The system resolver consults more than the DNS cache. A hosts entry overrides DNS, and a VPN can add a scoped resolver for a domain.",
                new Wrong("Reinstall macOS to restore the default network configuration", "That's a drastic step for a problem that one text file or a resolver entry can explain."),
                new Wrong("Run the same flush command again until the result changes", "The cache is already clear, so repeating the flush won't change a persistent source."),
                new Wrong("Ask the DNS team to lower the record's TTL and wait for the old answer to age out of the cache", "DNS is already answering correctly, so a TTL change can't affect this Mac's answer."))),

        new(
            "mac-8021x-auth-drops",
            ScenarioCategory.MacNetworkIssues,
            "802.1X Wi-Fi drops on MacBooks",
            "MacBooks on the WPA2-Enterprise SSID CorpSecure started dropping off Wi-Fi about 8 hours after connecting, and they fail to rejoin. It started the day after the RADIUS server's certificate was renewed. Windows laptops are fine. The Macs connect using a configuration profile from your MDM.",
            "802.1X authenticates a device before it's given network access. With PEAP or EAP-TLS, the client validates the RADIUS server's certificate during the TLS handshake. On macOS, the eapolclient process handles this, and an MDM profile normally defines which certificates and server names to trust. Macs stay connected on an existing session, then fail when reauthentication happens, which explains drops long after the certificate change. Here the 8-hour pattern matches the session timeout. Windows machines were fine because they trust the new chain another way. The right fix is to push an updated profile containing the new CA chain and trusted server names. Telling users to click Trust or turning off validation trains people to accept a fake access point, which is how credentials get stolen. On Windows NPS, event 6272 means access was granted and 6273 means it was denied.",
            new Step(
                "Where do you gather evidence first?",
                "Match the drop times in the Mac's eapolclient logs against the RADIUS server's logs",
                "Authentication has two sides. The Mac shows what the client saw, and RADIUS shows why the request was accepted or rejected.",
                new Wrong("Forget the network on each Mac and then rejoin it manually", "This removes evidence and may not work on a network managed by a profile."),
                new Wrong("Replace the access points that the affected Macs are using, starting with the busiest floor of the office", "Windows laptops work on the same access points, so the hardware isn't the likely cause."),
                new Wrong("Check the DHCP lease for each Mac on the staff VLAN", "802.1X happens before an IP address is assigned, and these drops are authentication failures.")),
            new Step(
                "The Mac logs show a TLS failure during EAP, and RADIUS logs an unknown CA alert from the client at each drop. Passwords are correct. What is the cause?",
                "The Macs' 802.1X profile doesn't trust the renewed RADIUS certificate chain",
                "An unknown CA alert means the client rejected the server's certificate. The timing matches the renewal, and the Macs are the ones using a fixed trust profile.",
                new Wrong("The users' directory passwords expired overnight, so the RADIUS server is rejecting every login", "Expired passwords produce a credential rejection, and only after the TLS tunnel forms."),
                new Wrong("The Wi-Fi pre-shared key was rotated on the access points", "WPA2-Enterprise uses 802.1X per user and has no pre-shared key."),
                new Wrong("The DHCP scope for the staff VLAN has run out of addresses", "A device needs to authenticate first, so this failure comes before DHCP.")),
            new Step(
                "Why did the Macs only drop about 8 hours later, and not the moment the certificate changed?",
                "Certificates are only checked when authenticating, and reauthentication happens at session timeout",
                "A connected session isn't re-validated continuously. The new certificate only comes into play at the next authentication, which fits the 8-hour pattern.",
                new Wrong("macOS checks certificates for revocation on a fixed daily schedule", "Revocation isn't what's failing, since the client reports an unknown CA."),
                new Wrong("DNS caching delayed the new certificate from reaching the Macs", "The certificate is presented in the EAP exchange, not looked up through DNS."),
                new Wrong("The access points stored the old certificate in their memory", "The access point only relays EAP. The RADIUS server presents the certificate.")),
            new Step(
                "What is the correct fix?",
                "Push an updated MDM profile with the new CA chain and trusted server names",
                "This restores proper validation on every managed Mac at once, and users don't need to make any trust decisions.",
                new Wrong("Tell users to click Trust when the certificate prompt appears", "Teaching users to accept certificate prompts exposes them to fake access points."),
                new Wrong("Turn off server certificate validation in the 802.1X profile so the Macs accept the renewed certificate", "Without validation, Macs may send credentials to any rogue access point using that SSID."),
                new Wrong("Move the MacBooks to the open guest SSID for the time being", "This drops access to internal resources and leaves the failure in place.")),
            new Step(
                "How do you confirm the fix has worked?",
                "Confirm the profile is installed, then verify NPS event 6272 through a full session timeout",
                "The fault appeared at reauthentication, so a successful grant after a full 8-hour session is the evidence that counts.",
                new Wrong("Check that a Mac connects again straight after the profile arrives, then close the ticket as resolved", "The first connection is useful, but it doesn't prove the 8-hour reauthentication passes."),
                new Wrong("Ask one user to say whether Wi-Fi feels more stable today", "A single impression can't prove a fault that shows up hours later."),
                new Wrong("Confirm the Wi-Fi icon in the menu bar shows full signal", "Signal strength has nothing to do with whether authentication succeeds."))),

        // --------------------------------------------------------------- Active Directory
        new(
            "ad-account-lockout-source",
            ScenarioCategory.ActiveDirectory,
            "Account keeps locking after a password change",
            "Dana changed her domain password on Monday. Since then her account locks out every 20 minutes or so, even while she's away from her desk. The help desk has unlocked it six times, and each unlock lasts only a few minutes.",
            "An account locks when the count of bad passwords passes the lockout threshold, so a lockout that repeats on a timer means something is still sending the old password. A domain controller that sees a bad password checks it with the PDC emulator, which always has the newest password, and lockouts are processed there. That makes the PDC emulator's Security log the place to look for event 4740, 'A user account was locked out'. Find that DC with (Get-ADDomain).PDCEmulator. The event's Caller Computer Name field names the computer the failed attempt came from. On that computer, event 4625 records each failed logon with its logon type: 2 is interactive, 3 is network, 4 is batch (scheduled tasks), 5 is service, and 10 is Remote Desktop. Usual culprits are scheduled tasks, services, mapped drives, saved entries in Credential Manager, disconnected Remote Desktop sessions, and mail apps on phones. Event 4740 is only logged when Audit User Account Management is enabled.",
            new Step(
                "The help desk wants to unlock the account a seventh time. What should happen first?",
                "Find what is still sending the old password before unlocking the account again",
                "A lockout on a timer has a machine behind it. Until that source is found, each unlock only lasts until its next attempt.",
                new Wrong("Reset Dana's password to something new and have her sign in again", "The stale credential is wrong against any new password, so the lockouts continue."),
                new Wrong("Raise the lockout threshold in the Default Domain Policy so that accounts take far longer to lock out", "This weakens password-guessing protection for every account to hide one fault."),
                new Wrong("Set Dana's password to never expire", "Expiry isn't involved. The password was already changed, and something kept the old one.")),
            new Step(
                "Where do you look to find out where the bad passwords are coming from?",
                "The Security log on the PDC emulator, filtered to event 4740 for Dana's account",
                "Lockouts are processed on the PDC emulator, and event 4740 there records the computer that the failed attempts came from.",
                new Wrong("The Application log on Dana's laptop", "Lockout events are security events written by domain controllers, and her laptop may not be the source."),
                new Wrong("The System log on each file server Dana uses, starting with the one she opens most often during the day", "This guesses at the source. The lockout event names it directly."),
                new Wrong("The DNS Server log on the nearest domain controller", "DNS logs record name resolution, not authentication failures.")),
            new Step(
                "Event 4740 shows Caller Computer Name: RECEPTION-PC02, a shared front-desk PC Dana covered last week. What next?",
                "On RECEPTION-PC02, read the 4625 events to see which logon type is failing",
                "Event 4625 on the source computer records each failed logon and its logon type, which shows what kind of thing is sending the password.",
                new Wrong("Disable the RECEPTION-PC02 computer account in Active Directory", "That takes a working front-desk PC off the domain and tells you nothing about the cause."),
                new Wrong("Reimage RECEPTION-PC02, because a PC that keeps sending bad passwords has clearly been infected by malware", "A leftover saved credential is far more likely, and reimaging destroys the evidence either way."),
                new Wrong("Delete Dana's roaming profile from the profile server", "Nothing ties the profile to the failed attempts, and this risks losing her data.")),
            new Step(
                "The 4625 events for Dana's account on that PC all show Logon Type 4, about 20 minutes apart. What is the likely cause?",
                "A scheduled task on that PC saved under Dana's account with her old password",
                "Logon type 4 is a batch logon, which is how scheduled tasks sign in. The regular interval matches a task trigger.",
                new Wrong("Someone at the front desk keeps typing Dana's password incorrectly", "Typing at the keyboard is an interactive logon, type 2, and people don't fail on a 20-minute timer."),
                new Wrong("A mapped network drive on Dana's own laptop is reconnecting with the credentials it saved before Monday", "That would be a network logon, type 3, and the caller would be her laptop."),
                new Wrong("Dana's phone is sending the old password to the mail server", "The caller would then be the mail or sync server, not the front-desk PC.")),
            new Step(
                "You find a report-export task running as Dana. How do you finish the ticket?",
                "Fix or remove the task's saved credentials, unlock the account, then watch for new 4740 events",
                "Removing the source stops the bad attempts. Watching for further lockout events proves that there wasn't a second source.",
                new Wrong("Unlock the account and close the ticket", "The task still holds the old password, so the account locks again at its next run."),
                new Wrong("Create a fine-grained password policy with a lockout threshold of 0 and apply it to Dana, so her account can't lock", "This removes lockout protection from her account and leaves the faulty task running."),
                new Wrong("Rename Dana's account so the task can no longer find it", "This breaks her sign-in and everything else tied to the name, to avoid editing one task."))),

        new(
            "ad-trust-relationship-failed",
            ScenarioCategory.ActiveDirectory,
            "Trust relationship with the domain failed",
            "After a bad software install, a technician reverted the virtual desktop VDI-ACCT07 to a checkpoint taken ten weeks ago. Now every domain user who tries to sign in to it gets 'The trust relationship between this workstation and the primary domain failed'.",
            "Every domain-joined computer has its own account in Active Directory, with a password that the computer and the domain both know. The computer uses it to build a secure channel to a domain controller, and domain sign-ins on that computer depend on the channel. By default the computer itself starts a password change every 30 days. Active Directory never expires a machine password, so a PC that was simply switched off for months still works. The usual real causes are restoring an old checkpoint or image, which brings back a password the domain has since replaced, or a computer account that was reset, deleted, or taken over by another machine with the same name. Test-ComputerSecureChannel reports whether the channel is healthy, and adding -Repair with a domain credential resets it in place. Reset-ComputerMachinePassword is the next option. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort. Don't delete the computer object, since a new one has a different SID and loses its group memberships.",
            new Step(
                "What does this error mean?",
                "The PC's secure channel is broken, as its machine account password doesn't match the domain's",
                "The computer proves its identity to the domain with its own account password. When the two sides disagree, the secure channel fails and so do domain sign-ins.",
                new Wrong("The user's domain password has expired", "An expired user password gives a prompt to change it, and it wouldn't affect every user on one PC."),
                new Wrong("An administrator has removed the forest trust to a partner organization, so its accounts can no longer sign in here", "Despite the wording, this message is about the computer's own account, not a trust between domains."),
                new Wrong("The domain controller's certificate has expired", "That would affect many computers at once, not one reverted virtual desktop.")),
            new Step(
                "Domain sign-ins fail on this PC. How do you get in to repair it?",
                "Sign in with a local administrator account",
                "A local account is checked against the PC's own account database, so it doesn't need the secure channel.",
                new Wrong("Sign in as a member of Domain Admins, since that group's rights let it bypass the secure channel", "A domain admin is still a domain account, and it needs the same broken channel to be authenticated."),
                new Wrong("Keep retrying until a different domain controller answers", "Every domain controller holds the same machine account password, so none will accept this PC."),
                new Wrong("Sign in with the user's Microsoft account", "A domain-joined PC with no such account set up has nothing to check it against.")),
            new Step(
                "How do you confirm that the secure channel is the problem?",
                "Run Test-ComputerSecureChannel -Verbose in an elevated PowerShell window",
                "The cmdlet tests the channel between the local computer and its domain, and returns False when it's broken.",
                new Wrong("Ping the domain controller's IP address", "A reply shows the network path works. It says nothing about whether the domain trusts this PC."),
                new Wrong("Run gpupdate /force and read through the output for computer policy errors, then compare it with a working PC", "Policy would fail, but only as a side effect. It doesn't test the channel itself."),
                new Wrong("Run ipconfig /flushdns and try the sign-in again", "The PC can already find the domain. What's wrong is what happens when it gets there.")),
            new Step(
                "The test returns False. What is the least disruptive fix?",
                "Run Test-ComputerSecureChannel -Repair -Credential with a domain account allowed to reset it",
                "This resets the machine account password on both sides in place, usually with no restart and with the computer object left intact.",
                new Wrong("Delete the computer object in Active Directory, then restart", "The PC then has no account at all, and a new object gets a new SID and loses its group memberships."),
                new Wrong("Move the PC to a workgroup, restart, join the domain again and restart once more, as that's the only supported repair", "Rejoining works, but it isn't the only way, and it costs two restarts for a problem one command can fix."),
                new Wrong("Reset the user's password in Active Directory", "User passwords were never the problem. The mismatch is on the computer's account.")),
            new Step(
                "The manager asks why reverting the checkpoint caused this. What do you say?",
                "The checkpoint restored an old machine password that the domain had since replaced",
                "The PC changed its password after the checkpoint was taken. Reverting put back the old one, while Active Directory kept the new one.",
                new Wrong("The VM had been off for more than 30 days, so Active Directory expired its machine account password", "The computer starts its own password changes, and Active Directory doesn't expire machine passwords for being offline."),
                new Wrong("Too many users typed wrong passwords on that PC", "Bad user passwords lock user accounts. They don't affect the computer's own account."),
                new Wrong("The revert gave the VM a different IP address", "The secure channel is based on the machine account password, not the address."))),

        new(
            "ad-replication-rpc-1722",
            ScenarioCategory.ActiveDirectory,
            "New accounts missing at a branch (replication error 1722)",
            "Accounts created this morning on HQ-DC01 can't sign in at the branch office, where PCs use the local domain controller BR-DC02. Password resets made at headquarters aren't working at the branch either. Internet access at the branch is fine.",
            "Domain controllers copy changes to each other, and when that stops, each site slowly works from a different version of the directory. Start with repadmin /replsummary for a forest-wide view of failures and how old they are, then repadmin /showrepl on the failing DC for each partition's last attempt, result, and last success. Error 1722, 'The RPC server is unavailable', is a connectivity error: the destination DC couldn't connect to the source DC. Microsoft notes that DNS lookup failures cause a large share of these, because DCs locate each other through records in the _msdcs zone. dcdiag /test:dns checks that, and nltest /dsgetdc tests the DC locator. A DC must use internal DNS servers that host the Active Directory zones. A public resolver doesn't hold those records, and an alternate server is only tried when the preferred one doesn't answer. Other causes of 1722 are blocked ports, namely TCP 135 for the endpoint mapper and the dynamic RPC range. Don't leave replication broken for long. A DC that stays out of sync past the tombstone lifetime, usually 180 days, can bring deleted objects back.",
            new Step(
                "Which command gives you the quickest overall view of replication health?",
                "repadmin /replsummary",
                "It summarizes failures and the largest replication delay for every domain controller, so you can see at once which ones are behind.",
                new Wrong("gpupdate /force", "This refreshes Group Policy on one computer. It doesn't report on replication between DCs."),
                new Wrong("netdom query fsmo", "This lists which DCs hold the operations master roles, not whether they are replicating."),
                new Wrong("dcpromo on BR-DC02, to demote it and promote it again so that it pulls down a completely fresh copy of the directory", "Rebuilding a DC before diagnosing it is drastic, and the new one would hit the same fault.")),
            new Step(
                "The summary shows BR-DC02 failing inbound replication for two days with error 1722. What does that error tell you?",
                "BR-DC02 can't connect to the RPC service on its source DC, so suspect DNS or the network path",
                "1722 means the RPC server is unavailable. It's a connection failure, not a problem with the directory data.",
                new Wrong("The two DCs disagree about a password, so the krbtgt account needs to be reset", "Nothing in a 1722 error points to Kerberos keys, and resetting krbtgt is a major change."),
                new Wrong("The directory database on BR-DC02 is corrupt", "Database faults produce different errors. This one says the other server couldn't be reached."),
                new Wrong("BR-DC02 has no permission to replicate", "A permissions failure is reported as error 5 or 8453, access denied.")),
            new Step(
                "What do you test first?",
                "Run dcdiag /test:dns /v on BR-DC02 and check it can resolve HQ-DC01's records",
                "DNS lookup failures are behind a large share of 1722 errors, so name resolution is the first thing to rule in or out.",
                new Wrong("Restart HQ-DC01 during the working day", "The source DC is serving headquarters normally, and this adds an outage without a diagnosis."),
                new Wrong("Run repadmin /syncall over and over until the error stops appearing in the output", "Forcing a sync that can't connect just repeats the same failure."),
                new Wrong("Seize the operations master roles onto BR-DC02", "Seizing roles is for a role holder that is permanently gone. It isn't a replication fix.")),
            new Step(
                "The DNS test fails. BR-DC02's network adapter lists 8.8.8.8 as its preferred DNS server, set during an ISP outage last week. What is the fix?",
                "Point BR-DC02 back at the internal AD DNS servers, then clear its resolver cache",
                "Only the internal DNS servers hold the domain's records. Clearing the cache with ipconfig /flushdns removes the failed lookups.",
                new Wrong("Add HQ-DC01's name and address to the hosts file on BR-DC02", "DCs find each other through several records in the _msdcs zone, which a single hosts entry can't replace."),
                new Wrong("Keep 8.8.8.8 as preferred and add an internal DNS server as the alternate, so that internet and domain names both resolve", "The alternate is only used when the preferred server doesn't answer, and 8.8.8.8 answers that the name doesn't exist."),
                new Wrong("Open TCP port 135 on the branch firewall", "The connection can't be attempted until the name resolves, and the DNS test already found the fault.")),
            new Step(
                "How do you confirm that replication has recovered?",
                "Force a sync, then check repadmin /replsummary for zero failures and a current last success",
                "A clean summary with a fresh success time shows the backlog has been delivered, including this morning's accounts.",
                new Wrong("Check that BR-DC02 answers a ping from HQ-DC01", "Ping worked while replication was failing, so it can't show that anything changed."),
                new Wrong("Ask the new users to try again tomorrow, and reopen the ticket if any of them still can't sign in at the branch", "This leaves verification to users and a day's delay, when one command answers it now."),
                new Wrong("Look for a green status icon in Server Manager", "That reflects local services and events, not whether partitions are up to date."))),

        new(
            "ad-gpo-not-applying",
            ScenarioCategory.ActiveDirectory,
            "Group Policy isn't applying to a security group",
            "A colleague built a GPO called Sales Drive Map with user settings and linked it to the Staff OU. To limit it to the sales team, they removed Authenticated Users from Security Filtering and added the Sales-Users group. Nobody in Sales gets the drive, and gpupdate /force doesn't help.",
            "A GPO applies to a user only if it is linked where the user's account lives, and the user has both Read and Apply group policy permissions on it. Security Filtering is a shortcut for granting those two permissions. There's one more requirement that catches people out. Since security update MS16-072 in June 2016, Windows fetches user policy with the computer's security context, where it used to use the user's. The computer account therefore needs Read permission on any GPO that applies to the signed-in user. By default it gets that through Authenticated Users, a group that includes computer accounts. Removing Authenticated Users from Security Filtering takes away Read as well as Apply. The fix is on the GPO's Delegation tab: add Authenticated Users or Domain Computers with Read only. That lets computers read the GPO without applying it to everyone. On the client, gpresult /h produces a report of which GPOs were applied or denied, and why. A Deny entry always beats an Allow.",
            new Step(
                "What do you do first on an affected sales PC?",
                "Run gpresult /h report.html as the user and read which GPOs were applied or denied",
                "The report shows what the client actually processed and the reason for anything it skipped, so you aren't guessing from the server side.",
                new Wrong("Restart the PC a few times", "If the policy can't be read, it fails the same way on every restart."),
                new Wrong("Run gpupdate /force on the domain controller that holds the PDC emulator role, and then wait 90 minutes", "Policy is processed on the client. Refreshing a DC's own policy does nothing for a sales PC."),
                new Wrong("Map the drive by hand on each sales PC", "This hides the fault and leaves a GPO that still doesn't work for the next person.")),
            new Step(
                "The report doesn't list Sales Drive Map as applied, and it flags an access denied error for user policy. The user is in Sales-Users and in the Staff OU. What do you check?",
                "The GPO's Delegation tab, to see whether computer accounts can read it",
                "The link and the group membership are right, so permissions on the GPO are what's left, and access denied points the same way.",
                new Wrong("Whether Block Inheritance is set on the Staff OU", "The GPO is linked to that OU directly. Block Inheritance only stops GPOs from higher up."),
                new Wrong("Whether the PC runs a newer Windows build than the domain controllers, since newer clients skip GPOs made on older servers", "There's no such rule. Clients process GPOs regardless of which server version created them."),
                new Wrong("The WMI filter on the Default Domain Policy", "A WMI filter on another GPO has no effect on this one.")),
            new Step(
                "The Delegation tab lists Sales-Users, Domain Admins, Enterprise Admins, SYSTEM and Enterprise Domain Controllers. Why does that break the GPO?",
                "User policy is now fetched as the computer, and no entry lets the PCs read this GPO",
                "Since MS16-072, the computer account needs Read on user GPOs. It had that through Authenticated Users until the group was removed.",
                new Wrong("Security filtering only accepts computer groups", "User groups are valid for filtering user settings. The missing piece is Read for computers."),
                new Wrong("Sales-Users is a global group, and filtering needs a domain local group", "Global security groups work for security filtering."),
                new Wrong("A GPO with no Authenticated Users entry in Security Filtering is treated as disabled, and every client in the domain skips it", "Filtering to other groups is supported. It just needs computers to keep Read access.")),
            new Step(
                "What is the right fix?",
                "On the Delegation tab, add Domain Computers or Authenticated Users with Read only",
                "Read without Apply group policy lets the PCs fetch the GPO, while Security Filtering still limits who it applies to.",
                new Wrong("Put Authenticated Users back into Security Filtering", "That grants Apply as well as Read, so the drive maps for everyone in the Staff OU again."),
                new Wrong("Add every sales user to the local Administrators group on their PC, so that their sessions are able to read the policy", "Local admin rights don't grant the computer account access to the GPO, and they create a security problem."),
                new Wrong("Set the GPO link to Enforced", "Enforced changes precedence over other GPOs. It can't make an unreadable GPO readable.")),
            new Step(
                "How do you confirm the fix?",
                "Run gpupdate /force on a sales PC, then confirm gpresult /r lists the GPO as applied",
                "Checking on a real sales PC as a sales user tests the exact path that was failing.",
                new Wrong("Check in the console that the GPO link is enabled", "The link was enabled throughout the fault, so it proves nothing."),
                new Wrong("Assume it worked if nobody in Sales has called back about the missing drive by the end of the week", "Silence isn't evidence, and it leaves users without the drive in the meantime."),
                new Wrong("Confirm that an administrator's PC gets the drive", "An administrator outside Sales-Users shouldn't get it, so that test can only mislead."))),

        // ----------------------------------------------------------------- Windows Server
        new(
            "winsrv-system-drive-full",
            ScenarioCategory.WindowsServer,
            "System drive full on a web server",
            "Monitoring reports that the C: drive on WEB-SRV01 has 1% free space. Windows Update is failing on the server, and the web application has started logging write errors.",
            "A full system drive stops updates, logging, and sometimes the applications themselves, so act quickly, but measure before deleting anything. List the largest folders first. On servers the space usually goes to logs that nobody rotates, such as IIS logs under C:\\inetpub\\logs\\LogFiles, along with dump files, temp folders and old installers. The WinSxS folder is the Windows component store. It holds the files Windows needs to service itself, and Microsoft warns that deleting files from it can leave a system unable to boot or update. Reduce it with the built-in tools. Dism /Online /Cleanup-Image /AnalyzeComponentStore reports how much can be reclaimed, and /StartComponentCleanup removes superseded component versions straight away, where the scheduled task waits 30 days. Adding /ResetBase removes every superseded version, and after that the updates already installed can't be uninstalled. Then fix the cause: put retention on the logs or move them to a data volume, and alert on free space well before it's critical.",
            new Step(
                "What do you do first?",
                "Find what is using the space by listing the largest folders on C:",
                "Measuring first shows where the space went, so that you remove the right thing and can explain the cause.",
                new Wrong("Delete the contents of C:\\Windows\\System32\\config", "That folder holds the registry hives. Removing them stops Windows from starting."),
                new Wrong("Restart the server to clear out temporary files", "A restart frees little, interrupts the site, and may not complete cleanly on a full disk."),
                new Wrong("Extend the C: volume straight away and close the alert, since every server needs a bigger disk as it gets older", "Without finding the cause, the new space fills the same way and the alert returns.")),
            new Step(
                "You find 38 GB of IIS logs going back four years in C:\\inetpub\\logs\\LogFiles, and a 14 GB WinSxS folder. What is the best first move?",
                "Archive or delete the old IIS logs, keeping whatever the app owner must retain",
                "Old web logs are the largest item and are safe to remove once retention needs are met. This frees space with no risk to Windows.",
                new Wrong("Delete the WinSxS folder", "WinSxS is the component store. Deleting it can leave the server unable to boot or install updates."),
                new Wrong("Turn off the page file and delete pagefile.sys", "This risks out-of-memory failures and removes the ability to capture a crash dump."),
                new Wrong("Stop the World Wide Web Publishing Service so no more logs can be written while you decide what to do", "This takes the website down, and the logs already on disk still fill the drive.")),
            new Step(
                "How do you safely reduce the WinSxS folder?",
                "Run Dism /Online /Cleanup-Image /AnalyzeComponentStore, then /StartComponentCleanup",
                "The analysis reports what can be reclaimed, and the cleanup removes superseded component versions in the supported way.",
                new Wrong("Delete its oldest subfolders in File Explorer", "Windows tracks those files. Removing them by hand breaks servicing and can stop the server booting."),
                new Wrong("Run sfc /scannow", "System File Checker repairs protected system files. It doesn't remove superseded components."),
                new Wrong("Take ownership of the folder from TrustedInstaller, then turn on NTFS compression for it from the Properties page", "Changing ownership of the component store breaks servicing, and the store already compresses unused components.")),
            new Step(
                "A colleague suggests adding /ResetBase to the cleanup. What is the trade-off?",
                "It removes every superseded component version, so installed updates can't be uninstalled",
                "ResetBase frees the most space, but it removes the rollback path for existing updates. Updates installed later can still be uninstalled.",
                new Wrong("It resets Windows to its original image and removes installed roles", "It only affects superseded component versions. Roles, features and data are untouched."),
                new Wrong("It clears the update history, so the server has to download and reinstall every update released since it was built", "Installed updates stay installed. What's lost is the ability to roll them back."),
                new Wrong("There is none, so always use it", "Losing the ability to uninstall a bad update matters on a production server.")),
            new Step(
                "Space is back to 45% free. How do you stop this from happening again?",
                "Set log retention or move logs off C:, and alert on free space much earlier",
                "The logs caused the incident, so they need a limit. Earlier alerts leave time to act before anything fails.",
                new Wrong("Schedule a restart every night", "Restarts don't delete logs, and they add downtime for no benefit."),
                new Wrong("Turn off IIS logging on the server completely, because nobody has read those log files in four years", "The logs are needed for troubleshooting and security investigations. Rotate them instead."),
                new Wrong("Disable Windows Update to stop WinSxS growing", "This leaves the server unpatched to save a small amount of space."))),

        new(
            "winsrv-rdp-cannot-connect",
            ScenarioCategory.WindowsServer,
            "Remote Desktop can't connect to a server",
            "Since last night's security hardening change, no administrator can open a Remote Desktop session to FILE-SRV03. The client reports 'Remote Desktop can't connect to the remote computer'. The server answers ping, and its file shares still work. You have access to its Hyper-V console.",
            "This error means the client never reached a working RDP listener, and Microsoft's guidance splits the causes into the listener and the network. With console access, run qwinsta on the server. A line for rdp-tcp in the Listen state means the listener is up. If it's missing, check that fDenyTSConnections is 0, both under HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server and in the policy path if one is set, and that the Remote Desktop Services service (TermService) is running. Next, connect from the console with mstsc to localhost. If that works, the server side is healthy and the fault is between client and server. Test-NetConnection with -Port 3389 from another machine tests the TCP port itself, which ping can't do because ping uses ICMP. A False result points to the Windows Firewall rules named Remote Desktop - User Mode (TCP-In and UDP-In), a network firewall, or a changed port number. Failures after that point, such as credentials, Network Level Authentication, certificates, or membership of Remote Desktop Users, produce different messages because the connection has already been made.",
            new Step(
                "The server is running. Where do you start?",
                "Sign in at the console and run qwinsta to check for an rdp-tcp listener in the Listen state",
                "This shows whether the server is accepting RDP connections at all, which splits the problem into listener and network.",
                new Wrong("Restart the server from Hyper-V Manager", "A restart interrupts file shares that are working and won't undo a configuration change."),
                new Wrong("Remove the Remote Desktop Services role and add it again", "Administrative RDP doesn't use that role, and reinstalling skips the diagnosis."),
                new Wrong("Delete each administrator's saved .rdp file and clear the cached credentials in Credential Manager on their workstations", "Every administrator fails the same way after a server change, so the clients aren't the cause.")),
            new Step(
                "qwinsta shows rdp-tcp in the Listen state. How do you tell a server fault from a network fault?",
                "Connect with mstsc to localhost from the console, then test port 3389 from another PC",
                "A local connection proves the RDP service works. Test-NetConnection with -Port 3389 from elsewhere then tests the path to it.",
                new Wrong("Ping the server from an administrator's PC", "Ping uses ICMP and already works. It can't show whether TCP 3389 is reachable."),
                new Wrong("Run tracert to the server and count the hops along the way to see whether the route has changed since yesterday", "Routing is fine, because ping and file shares work. The question is one TCP port."),
                new Wrong("Check that the administrators' passwords haven't expired", "An expired password produces a credentials message after connecting, not this error.")),
            new Step(
                "The localhost session works. From a PC on the same subnet, the port test returns TcpTestSucceeded: False. What does this show?",
                "Something is blocking TCP 3389 before the listener, most likely the server's own firewall",
                "The service is healthy and the port is unreachable from the same subnet, which leaves little other than the host firewall.",
                new Wrong("The Remote Desktop Services service has stopped", "The localhost session used that service a moment ago, so it's running."),
                new Wrong("The administrators aren't members of the Remote Desktop Users group", "Group membership is checked after the connection is made, so the port test would succeed."),
                new Wrong("The server's Remote Desktop certificate has expired, which stops the TCP connection from being opened in the first place", "Certificates are exchanged after the TCP connection opens, so a bad one can't fail a port test.")),
            new Step(
                "In Windows Defender Firewall on the server, the rule Remote Desktop - User Mode (TCP-In) is disabled. What is the fix?",
                "Enable the Remote Desktop firewall rule group for the profiles the server uses",
                "The built-in rules allow exactly the traffic RDP needs. Get-NetFirewallRule -DisplayGroup \"Remote Desktop\" | Set-NetFirewallRule -Enabled True does it in one line.",
                new Wrong("Turn Windows Firewall off on all profiles and leave it off", "Turning it off briefly can confirm a diagnosis, but leaving it off removes protection from every port."),
                new Wrong("Change the RDP port to 3390 in the registry so that the sessions avoid the rule that was disabled", "No rule allows 3390 either, so the connection is still blocked."),
                new Wrong("Add a rule that allows all inbound TCP ports", "This opens the whole server to fix one port.")),
            new Step(
                "How do you finish the job?",
                "Repeat the port test, connect by RDP, and correct the hardening change so it can't recur",
                "The same tests that failed should now pass. Fixing the script or GPO behind the change stops it from disabling the rule again.",
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
                "Error 1069 is generic. The Service Control Manager event states which account failed to log on and why.",
                new Wrong("The Setup log, because the fault appeared straight after patching and a failed update is the likeliest cause", "The message says logon failure. The restart only exposed it, and the Setup log won't explain a logon."),
                new Wrong("The vendor's installation log from two years ago", "It describes how the service was installed, not why it can't sign in today."),
                new Wrong("The DNS Server log on a domain controller", "Name resolution isn't mentioned anywhere in this error.")),
            new Step(
                "Event 7038 says the service was unable to log on as CORP\\svc-sync with the currently configured password, because the user name or password is incorrect. What does this mean?",
                "The password saved in the service no longer matches the account's password in the domain",
                "The event names the configured password as the failure, and the account's password was rotated last week.",
                new Wrong("The account lacks the Log on as a service right", "That is event 7041, whose text says the user has not been granted the requested logon type."),
                new Wrong("The account has been disabled", "Event 7038 has separate wording for that, saying the account is currently disabled."),
                new Wrong("An update replaced the service's program file, and the Service Control Manager can no longer verify its signature", "The event describes a logon with a password. It says nothing about the program file.")),
            new Step(
                "The password was rotated a week ago. Why did the service only fail on Sunday?",
                "A running service keeps its logon session, so the saved password is only used at the next start",
                "The service signed in before the rotation and kept running. The restart was the first time Windows tried the saved password again.",
                new Wrong("Password changes take about a week to replicate between domain controllers", "Password changes replicate urgently, within seconds or minutes."),
                new Wrong("Windows keeps service passwords in a cache for seven days, then asks a domain controller for the new one", "Windows never fetches a new password for an ordinary account. An administrator has to enter it."),
                new Wrong("The update reset every service password", "Windows updates don't change the credentials saved for services.")),
            new Step(
                "What is the right fix?",
                "Enter the new password on the service's Log On tab, then start the service",
                "This brings the saved password back in line with the domain, which is the mismatch the event described.",
                new Wrong("Switch the service to Local System", "Local System has full control of the server and none of the account's permissions on other systems."),
                new Wrong("Set svc-sync's password in Active Directory back to the old value, without involving the security team", "This reverses a security control and breaks anything already updated to the new password."),
                new Wrong("Change the startup type to Automatic (Delayed Start)", "A delayed start helps when the domain can't be contacted at boot. Here the password is wrong.")),
            new Step(
                "How do you stop the next password rotation from causing the same outage?",
                "Move the service to a group managed service account, whose password is handled automatically",
                "With a gMSA, Active Directory rotates the password and the server retrieves it, so there's no saved password to go stale.",
                new Wrong("Set the password to never expire and exclude the account from every future rotation", "An unchanging password on a service account is exactly the risk rotation exists to remove."),
                new Wrong("Save the password in a text file on the server", "Anyone who can read the file can then use the account."),
                new Wrong("Add svc-sync to the Domain Admins group", "Group membership doesn't update a saved password, and this grants enormous privileges."))),

        new(
            "winsrv-share-access-denied",
            ScenarioCategory.WindowsServer,
            "Access denied on a file share",
            "Tomas joined Accounting today and gets 'Access is denied' when he opens \\\\FILE-SRV01\\Finance. His colleagues can open it. He was added to the Finance-RW group an hour ago and has been signed in since this morning.",
            "A shared folder has two sets of permissions. Share permissions apply only to access over the network, and NTFS permissions apply to every kind of access. Over the network both are evaluated and the more restrictive result wins, so Full Control on the share with Read in NTFS gives Read. Common practice is to keep share permissions broad, for example Authenticated Users with Full Control or Change, and to do the real control in NTFS with groups. Within one set, a user's permissions from all their groups add up, but a Deny entry beats any Allow. The Effective Access tab under Advanced Security Settings calculates the NTFS result for a named user. Group membership has its own catch. A user's access token, which lists their groups, is built when they sign in. Adding someone to a group changes Active Directory, but not a token that already exists, so the user has to sign out and back in. whoami /groups shows the groups in the current token. Grant access to groups and not to individual accounts, or the permissions become unmanageable.",
            new Step(
                "How do share permissions and NTFS permissions combine when someone connects over the network?",
                "Both are checked, and the more restrictive of the two results applies",
                "A user has to pass both. Whichever set allows less decides what they can do.",
                new Wrong("Share permissions override NTFS permissions", "Neither set overrides the other. Each can only narrow the result."),
                new Wrong("Whichever of the two is more generous applies", "It's the reverse. Access is limited to what both sets allow."),
                new Wrong("NTFS permissions are skipped for network access, and they only apply to people signed in at the server itself", "NTFS permissions apply to every kind of access. It's share permissions that only apply over the network.")),
            new Step(
                "Where do you look to see what access Tomas should have?",
                "On the server, review the share permissions and the folder's Security tab, with Effective Access",
                "These are the two sets that decide access. Effective Access calculates the NTFS result for his account.",
                new Wrong("In the Default Domain Policy", "Domain policy doesn't hold permissions for individual folders."),
                new Wrong("In the folder's Previous Versions tab", "That lists shadow copies for restoring files. It has nothing to do with permissions."),
                new Wrong("In the output of chkdsk on the data volume, in case file system errors have damaged the permissions on that folder", "His colleagues can open the same folder, so the file system isn't at fault.")),
            new Step(
                "The share grants Authenticated Users Full Control, and NTFS grants Finance-RW Modify. On Tomas's PC, whoami /groups doesn't list Finance-RW. What is the cause?",
                "His access token was built when he signed in, before he was added to the group",
                "The token is a snapshot of group membership at sign-in. The server checks his token, and it doesn't contain the group yet.",
                new Wrong("NTFS permissions take up to 24 hours to reach new group members", "There's no such delay. The permission is on the group, and his token doesn't carry the group."),
                new Wrong("The share has to be removed and created again every time the membership of one of its groups changes", "Shares refer to groups by their identifier, so membership changes need nothing done to the share."),
                new Wrong("Replication is failing between FILE-SRV01 and his PC", "Member servers and PCs don't replicate the directory. Only domain controllers do.")),
            new Step(
                "What is the fix?",
                "Have Tomas sign out and sign back in, so that he gets a new token",
                "Signing in again builds a token that includes Finance-RW, and the existing permissions then let him in.",
                new Wrong("Grant Tomas's own account Full Control on the folder", "It bypasses the group, gives him more than his colleagues have, and leaves a one-off entry to maintain."),
                new Wrong("Add Everyone with Full Control in NTFS", "This opens the Finance folder to every user on the network."),
                new Wrong("Restart FILE-SRV01, so that every connected user is forced to collect fresh group memberships from the domain", "The stale token is on his own session, so a server restart interrupts everyone and fixes nothing.")),
            new Step(
                "Later, auditors need read-only access to the same folder. How do you set it up?",
                "Give an Auditors group Read & execute in the folder's NTFS permissions",
                "The share already allows more than that, so NTFS decides the result. A group keeps the permission manageable as auditors come and go.",
                new Wrong("Add a Deny Full Control entry for the auditors on the share", "Denying Full Control denies every permission, including Read, and Deny beats any Allow."),
                new Wrong("Make a copy of the folder for the auditors", "The copy goes out of date immediately, and it doubles the sensitive data to protect."),
                new Wrong("Create a second, read-only share for the same folder and grant the auditors Modify in NTFS so they can open it", "They could still reach the original share, where NTFS Modify would let them change files."))),
    ];
}
