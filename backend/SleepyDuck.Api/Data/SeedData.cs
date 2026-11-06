using System.Security.Cryptography;
using System.Text;
using SleepyDuck.Api.Models;

namespace SleepyDuck.Api.Data;

/// <summary>
/// Microsoft 365 admin troubleshooting deck. EF Core data seeding needs stable keys, so
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
                new Wrong("Ask the supplier to resend the invoice from a personal mailbox", "This sidesteps the problem without finding the cause, and it teaches staff to accept invoices from unfamiliar addresses.")),
            new Step(
                "Message trace is open. How do you scope the search?",
                "Set the sender to billing@northwind-supply.example, the recipient to Priya, and a date range covering the missing invoices",
                "Filtering on both parties and the right window returns only the relevant messages, and the sender and recipient appear exactly as Exchange Online recorded them.",
                new Wrong("Search with Priya as the sender over the last 24 hours", "That finds what Priya sent, not what she should have received."),
                new Wrong("Run an unfiltered trace for the whole organization", "It returns a large volume of unrelated results and may hide the message you need."),
                new Wrong("Run a content search for 'invoice' in Microsoft Purview", "Content search only looks at items stored in mailboxes and other locations, so it can't find a message that was never delivered.")),
            new Step(
                "The trace shows the supplier's messages with the status Quarantined, flagged as high confidence spam. What is your next step?",
                "Open Quarantine in the Microsoft Defender portal, review the messages, and release them if they are legitimate",
                "Quarantined messages are held in the Defender portal. Reviewing them first confirms that they really are from the supplier before they reach the user.",
                new Wrong("Tell Priya to look in her Deleted Items folder", "A quarantined message was never delivered to a mailbox folder, so it won't be there."),
                new Wrong("Disable the anti-spam policy for the tenant", "This removes protection for everyone to fix a false positive affecting a single sender."),
                new Wrong("Ask the supplier to resend and wait for it to clear", "The same filter verdict will probably apply again, and the held invoices remain stuck.")),
            new Step(
                "The messages were legitimate. How do you stop this from recurring?",
                "Report them as false positives through Submissions in the Defender portal and create a Tenant Allow/Block List allow entry from the submission",
                "Submitting teaches the filters, and the allow entry is targeted and expires, so it is the Microsoft-recommended way to handle a false positive.",
                new Wrong("Add northwind-supply.example to the allowed domains in the anti-spam policy", "Allowed domains are a known risk because spoofed mail from the domain can bypass spam filtering."),
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
                new Wrong("The partner's mail server rejected Marco's domain", "The block happens on your side of the connection before the message leaves Exchange Online."),
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
                new Wrong("Set automatic forwarding to On in the Default outbound policy", "That permits external forwarding for all users, including any compromised account."),
                new Wrong("Give Marco the Global Administrator role", "Admin roles don't exempt a user from mail policy, and this grossly over-privileges the account."),
                new Wrong("Remove the outbound policy", "The Default policy can't be deleted, and removing protection wouldn't be the right fix anyway.")),
            new Step(
                "How do you confirm the fix?",
                "Send a test message that triggers the rule, then confirm delivery with a message trace",
                "Policy changes can take a little while to apply. A trace proves that the forwarded copy left the tenant and was delivered.",
                new Wrong("Ask the partner to mark Marco's address as safe", "The problem was never on the partner's side."),
                new Wrong("Close the ticket as soon as the policy is saved", "A saved setting isn't proof, particularly when policies take time to apply."),
                new Wrong("Check Marco's Sent Items for the forwarded copy", "Server-processed forwards typically don't appear in Sent Items, so this proves nothing."))),

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
                new Wrong("Exchange admin center > Recipients > Contacts", "A mail contact is an external address entry with no sign-in or license."),
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
                new Wrong("Block Dana with a Conditional Access policy", "Conditional Access blocks sign-ins to apps, but Dana would still be licensed and it doesn't change the entitlement HR asked about."),
                new Wrong("Assign Exchange Online Plan 1 instead of E3", "This loses the rest of E3, such as the Office apps, which Dana is meant to have."),
                new Wrong("Delete the SharePoint site collection for the Warehouse team", "This destroys content for others and doesn't affect Dana's entitlement.")),
            new Step(
                "How do you verify the result before Monday?",
                "Open Dana's user pane and check Licenses and apps to confirm that those two apps are off",
                "This view shows what's actually provisioned for the user, including each service plan's state.",
                new Wrong("Check the Billing > Your products page", "It shows tenant-level subscriptions and license counts, not one user's service plans."),
                new Wrong("Ask Dana to sign in and report back on Monday", "That's too late, and it relies on the user to discover a misconfiguration."),
                new Wrong("Review the Exchange admin center for the mailbox", "This confirms that a mailbox exists but says nothing about SharePoint or Viva Engage."))),

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
                new Wrong("Assign the license to a distribution list", "Group-based licensing needs a security-enabled group or Microsoft 365 group. A distribution list isn't supported.")),
            new Step(
                "When you assign E3 to the group, how do you exclude Viva Engage?",
                "Turn off the Viva Engage service plan in the group's license assignment options",
                "The group's assignment options apply the same service plan configuration to every member.",
                new Wrong("Add a second group that removes the plan", "Assignments are additive. A second group can't subtract what the first one grants."),
                new Wrong("Remove Viva Engage from each user afterward", "Manual per-user edits are the work you're trying to eliminate, and each joiner would need it too."),
                new Wrong("Disable Viva Engage for the tenant", "This affects every department, not just Marketing.")),
            new Step(
                "Three users show a license error. Where do you find out why?",
                "On the group's Licenses page in Microsoft Entra ID, review the users with assignment errors",
                "Entra ID records assignment errors per user in the group's licensing view, along with the reason for each.",
                new Wrong("In the Exchange message trace", "Message trace records mail flow, not licensing."),
                new Wrong("In the Sign-in logs for those users", "Sign-in logs show authentication attempts, not license processing."),
                new Wrong("In Microsoft Purview Audit, by searching for failed logons", "Failed logons aren't related to a license assignment error.")),
            new Step(
                "The error says the users have no usage location. How do you fix it?",
                "Set the usage location on those users, then reprocess the assignment if it doesn't retry by itself",
                "Once the missing property is set, the group can assign the license. Reprocessing gets it done without waiting.",
                new Wrong("Remove them from the group permanently", "They still need the license, so this just abandons the requirement."),
                new Wrong("Purchase three additional E3 licenses", "The cause is a missing attribute, not a shortage of licenses."),
                new Wrong("Change the tenant's default language", "Language settings have no effect on usage location."))),

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
                new Wrong("SharePoint admin center > Term store", "The term store manages metadata and doesn't create groups."),
                new Wrong("Defender portal > Threat policies", "Threat policies configure security protection, not groups."),
                new Wrong("Exchange admin center > Mail flow > Rules", "Rules process messages and can't create a group.")),
            new Step(
                "What must you assign when creating the group?",
                "At least one owner, ideally two, who will manage its members",
                "Owners keep the group's membership and settings current without admin involvement, and a second owner guards against orphaning it.",
                new Wrong("A Global Administrator as the only member", "A group is meant to be run by its team, and this over-involves a privileged account."),
                new Wrong("A dedicated E5 license for the group", "A group doesn't need its own license."),
                new Wrong("A retention label on each member", "Retention labels apply to content, not to group membership.")),
            new Step(
                "Partners report that their emails to the group are being rejected. What should you change?",
                "Turn on the setting that allows people outside the organization to email the group",
                "By default, a Microsoft 365 Group only accepts mail from internal senders. This setting opens it to external senders.",
                new Wrong("Add each partner as a full member of the tenant", "That grants far more access than the requirement calls for."),
                new Wrong("Create a new inbound connector for the partners", "Connectors handle routing with specific organizations. The rejection comes from the group's own setting."),
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
                new Wrong("Microsoft 365 admin center > Roles > Role assignments", "Admin roles grant administrative rights, not sending rights on a single group."),
                new Wrong("Microsoft Entra ID > App registrations", "App registrations are for applications and have no connection to mail delegation."),
                new Wrong("Outlook on the web > Aisha's signature settings", "A signature changes the message text, not the sender's permissions.")),
            new Step(
                "You'd rather script this. Which PowerShell command is right?",
                "Add-RecipientPermission -Identity support@contoso.example -Trustee aisha@contoso.example -AccessRights SendAs",
                "In Exchange Online, Add-RecipientPermission is the cmdlet that grants Send As.",
                new Wrong("Add-MailboxPermission -Identity support@contoso.example -User aisha@contoso.example -AccessRights FullAccess", "This grants Full Access to a mailbox, and a distribution list doesn't have one."),
                new Wrong("Set-DistributionGroup -Identity support@contoso.example -GrantSendOnBehalfTo aisha@contoso.example", "This grants Send on Behalf, which reveals Aisha's name."),
                new Wrong("Add-DistributionGroupMember -Identity support@contoso.example -Member aisha@contoso.example", "Membership means Aisha receives the group's email, not that Aisha can send as it.")),
            new Step(
                "Aisha still can't send as the group right after you grant it. What's the most likely next step?",
                "Allow time for the permission to replicate, then have Aisha choose the support address in the From field",
                "Permissions can take a while to take effect, and Outlook sends from the user's own address unless the From field is changed.",
                new Wrong("Grant the Exchange Administrator role to Aisha", "An admin role is unnecessary and wouldn't fix replication timing."),
                new Wrong("Recreate the distribution list", "This disrupts existing members and addresses without addressing the cause."),
                new Wrong("Convert the distribution list to a shared mailbox immediately", "A migration isn't justified just because a permission hasn't propagated yet."))),

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
                new Wrong("An Outlook inbox rule on each mailbox", "Client rules are per mailbox, can be changed by users, and can't insert a disclaimer."),
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
                new Wrong("Enforce immediately and wait for user complaints", "A mistake in the condition could affect every message before anyone notices."),
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
                new Wrong("Check the affected users' Outlook view settings", "This happens to every recipient of that vendor's mail, so individual client settings aren't the cause.")),
            new Step(
                "The trace shows that a rule named 'Vendor Bypass' matched and your disclaimer rule didn't. What do you check?",
                "The priority order of the two rules, and whether 'Vendor Bypass' has 'Stop processing more rules' turned on",
                "A higher-priority rule with this option on prevents later rules from running for messages it matches.",
                new Wrong("Whether the vendor's domain has expired", "An expired domain wouldn't explain the mail arriving and matching a rule."),
                new Wrong("Whether the disclaimer text is over a length limit", "That wouldn't produce a failure for just one sender."),
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
    ];
}
