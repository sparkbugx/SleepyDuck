using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdvancedM365Scenarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScenarioCards",
                columns: new[] { "Id", "Category", "ProblemStatement", "TechnicalExplanation", "Title" },
                values: new object[,]
                {
                    { new Guid("0a9c0e04-1982-c0cb-7c90-748d8bba604c"), "GroupManagement", "Mei owns the Vendor Onboarding team in Microsoft Teams. When Mei types partner@fabrikam.example to add a member, Teams shows 'We didn't find any matches' and never offers to add a guest. Guest access is On in the Teams admin center, and fabrikam.example is an approved sharing domain.", "Adding a guest to a team requires several settings to agree. Guest access must be on in the Teams admin center, the Microsoft Entra external collaboration settings must let the owner invite guests, and the Microsoft 365 Groups settings must allow owners to add people outside the organization. A sensitivity label applied to the team can also block guests for that one team, because container labels control the group's guest setting. If any layer says no, Teams doesn't offer 'Add as a guest'. Testing whether the problem affects every team or only one tells you right away whether to look at a tenant-wide setting or at the team's label. A label reflects a classification decision, so changing it needs the data owner's approval. After accepting the invitation, the guest also has to switch to the host organization in Teams before the team appears.", "Team owner can't add a guest" },
                    { new Guid("342cc552-23d1-4282-8205-22cb79b47a8e"), "MailFlowRules", "At 10:40, three users report an email titled 'Payroll update required' that links to a fake Microsoft sign-in page. It came from hr-notice@payroll-portal.example and passed SPF and DKIM for that domain. The tenant has Microsoft Defender for Office 365 Plan 2.", "Threat Explorer in Defender for Office 365 Plan 2 finds every copy of a campaign by sender, subject or URL, and shows each copy's delivery location along with URL clicks recorded by Safe Links. You can act on the results directly, for example with a soft delete, and the Action center tracks the remediation. Zero-hour auto purge (ZAP) can also remove delivered messages when a verdict changes, but you can't rely on its timing during an active campaign. A sender that passes SPF and DKIM for its own lookalike domain isn't spoofing you, so your own DMARC policy won't stop it, and the Tenant Allow/Block List is the place to block the domain and the URL. Anyone who clicked through to the credential page may have given away a password, so reset it, revoke their sessions and review their sign-ins.", "Responding to a credential phishing campaign" },
                    { new Guid("bf707aa9-af50-2825-fddb-590cb6d999eb"), "EmailDelivery", "Grace, the CFO's assistant, says replies from the bank about a pending wire transfer never arrive, but the bank insists it sent them. Message trace shows every reply as Delivered to Grace's mailbox. Security wants the ticket worked entirely from PowerShell so that every action is scripted and recorded.", "When message trace says Delivered but the user never sees the message, something inside the mailbox moved or deleted it after delivery. In business email compromise, attackers commonly create inbox rules that move mail matching words such as 'bank', 'wire' or 'invoice' to a folder nobody checks, and these rules are often hidden from Outlook. Get-InboxRule with -IncludeHidden returns rules that Outlook doesn't show, and Get-Mailbox reveals mailbox-level forwarding set through ForwardingSmtpAddress. Removing the rule only fixes the symptom. The account has to be treated as compromised: reset the password and revoke its sessions so existing refresh tokens stop working, or the attacker can simply create the rule again. Connect with the Exchange Online PowerShell V3 module, because Basic authentication for remote PowerShell is turned off and the MSOnline module is retired.", "Hidden inbox rule found with PowerShell" },
                    { new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750"), "GroupManagement", "The Project Atlas team keeps its files on a Microsoft 365 Group site. Owners can share with partners at adatum.example, but sharing with a new consultant at fabrikam.example fails with 'Your organization's policies don't allow you to share with these users.' Organization-level external sharing is set to New and existing guests.", "SharePoint and OneDrive sharing works in layers. The organization-level setting in the SharePoint admin center is the most permissive level allowed, and each site can match it or be more restrictive. Domain restrictions apply on top of both, either for the whole organization under Policies > Sharing ('Limit external sharing by domain') or for a single site. Sharing with a domain that isn't on an allow list fails with this policy message, which is why one partner works and another doesn't. SharePoint also uses Microsoft Entra B2B for guests, so an allow or deny list in the Entra external collaboration settings can block the invitation too, and the two lists should agree. The narrow fix is adding the approved domain to the allow list, not loosening the sharing level or removing the restriction for everyone.", "External sharing blocked for one partner" },
                    { new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9"), "UserLicenseManagement", "Luis from Sales is at a client site in Lisbon and can't sign in to Outlook or Teams. The browser shows error AADSTS53003, 'Access has been blocked by Conditional Access policies'. No MFA prompt appears, and the same account worked from the office yesterday.", "Microsoft Entra ID evaluates every sign-in against the Conditional Access policies that apply to it, and the sign-in log records the result of each policy. AADSTS53003 means a policy's grant control was Block, which is why no MFA prompt appeared: a block is applied before any challenge is offered. Country-based named locations are resolved from the sign-in's public IP address, so a user abroad falls outside an 'allowed countries' location. The supported way to make an exception is an exclusion group with a documented end date, ideally governed by an access review. Disabling the policy, switching it to Report-only, or trusting a hotel IP weakens protection for everyone. The What If tool evaluates a simulated sign-in against every policy, so you can prove the exception works before the user tries again.", "Conditional Access blocks a traveling user" }
                });

            migrationBuilder.InsertData(
                table: "SequentialSteps",
                columns: new[] { "Id", "CorrectAction", "CorrectOptionId", "Explanation", "Order", "Prompt", "ScenarioCardId" },
                values: new object[,]
                {
                    { new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"), "Treat both accounts as compromised: reset passwords, revoke sessions, and review sign-ins", new Guid("e1b4ccce-9836-e4aa-9fec-348b544d8ead"), "A click through to a credential page may have captured a password, and revoking sessions cuts off any tokens the attacker already has.", 4, "Explorer's URL clicks show that two users clicked through the Safe Links warning. What do you do next?", new Guid("342cc552-23d1-4282-8205-22cb79b47a8e") },
                    { new Guid("063e942b-84c2-66c3-3815-01f571f03414"), "Add Luis to a travel exclusion group that the policy excludes, with a set removal date", new Guid("036ac3e0-c3b8-9245-68ef-35468cb7a221"), "The exception covers only the approved user for the approved period, and the policy keeps protecting everyone else.", 3, "Management approves access for the two-week trip. How do you apply it?", new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9") },
                    { new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"), "The policy blocked the sign-in because Portugal isn't in the allowed countries location", new Guid("91a68799-ba2d-22f3-5d17-733430c93f96"), "Country locations come from the sign-in's public IP. Luis's IP maps to Portugal, which the policy blocks.", 2, "The log shows error 53003, location Portugal, and the policy 'Block sign-ins outside allowed countries' with the result Failure. What is the cause?", new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9") },
                    { new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"), "Use What If for Luis with Portugal as the country, and check the block policy isn't applied", new Guid("47535515-8039-9bc4-c932-4a40b9b40605"), "What If simulates the sign-in against every policy, so it proves the exclusion works. Luis's next sign-in log entry then confirms it in production.", 4, "How do you confirm the exception before Luis tries again?", new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9") },
                    { new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"), "Open Luis's failed sign-in in the Entra sign-in logs and review its Conditional Access tab", new Guid("7d6513d9-9842-821b-5d57-503f7049d217"), "The sign-in log entry shows the error code, the location, and the result of every policy evaluated, including the one that blocked access.", 1, "Where do you find out why the sign-in was blocked?", new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9") },
                    { new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"), "Add fabrikam.example to the allowed domains, and to the Entra B2B allow list if one is used", new Guid("637ec6aa-8892-4b6e-08c6-3ea3d727db9f"), "This admits exactly the approved partner, and keeping the Entra list in step prevents a second block when the guest is invited.", 3, "Security approves Fabrikam as a partner. What do you change?", new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750") },
                    { new Guid("497b1140-0863-3bb1-9706-231416a7b32c"), "Remove the rule, clear any mailbox forwarding with Set-Mailbox, reset the password, and run Revoke-MgUserSignInSession", new Guid("4d4327bd-d2ba-2c3d-2e6c-0b3c2f23521a"), "Removing the rule and forwarding stops the hiding, while the password reset and session revocation lock out the attacker so the rule can't be recreated.", 4, "How do you contain this from PowerShell?", new Guid("bf707aa9-af50-2825-fddb-590cb6d999eb") },
                    { new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"), "Switch to the Contoso organization in Teams, using the account switcher on the profile picture", new Guid("b7df667c-36a7-5b9c-1ef3-b94e1bba3d31"), "Guest membership lives in the host tenant, and Teams only shows that tenant's teams after the guest switches to it.", 4, "The guest accepted the invitation but says the team doesn't show up in Teams. What do you tell them?", new Guid("0a9c0e04-1982-c0cb-7c90-748d8bba604c") },
                    { new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"), "The mailbox is probably compromised, and an attacker created the rule to hide the bank's replies", new Guid("3844b8c9-e9c0-bfe2-d6da-ffb16c2e6cbc"), "A one-character name, a hidden rule, and finance keywords moved to an unused folder are classic signs of business email compromise.", 3, "The output shows a hidden rule named '.' that moves messages containing 'bank' or 'wire' to RSS Subscriptions and marks them as read. What does this indicate?", new Guid("bf707aa9-af50-2825-fddb-590cb6d999eb") },
                    { new Guid("6d2abeae-2397-c7a3-b761-089f45177081"), "Get-InboxRule -Mailbox grace@contoso.example -IncludeHidden | Format-List Name,Description", new Guid("b4d0af24-66be-f9e3-2f54-ba667ea2551c"), "Inbox rules act after delivery, and -IncludeHidden returns rules that Outlook doesn't display. The Description property spells out each rule's conditions and actions.", 2, "Trace already shows Delivered. Which command shows what happens to the mail inside the mailbox?", new Guid("bf707aa9-af50-2825-fddb-590cb6d999eb") },
                    { new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"), "Search Threat Explorer in the Defender portal by sender address and subject for today", new Guid("124ca41d-746d-d99c-beb1-5876e3769954"), "Explorer returns every matching message with its recipients, delivery location and URL clicks in one view.", 1, "How do you find everyone who received the message?", new Guid("342cc552-23d1-4282-8205-22cb79b47a8e") },
                    { new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"), "Select the messages in Explorer, take a Soft delete action, and track it in the Action center", new Guid("b1bcbdba-725f-b6c4-780e-a56f3b78800f"), "Remediating from Explorer pulls delivered copies out of every mailbox at once, and the Action center records the result.", 2, "Explorer shows 140 recipients, with 128 copies still in Inboxes. How do you remove them?", new Guid("342cc552-23d1-4282-8205-22cb79b47a8e") },
                    { new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"), "Have an owner share again, then confirm the consultant can redeem the invitation and open the file", new Guid("fa35dc3d-26ac-ad12-e8af-2f41da5a7254"), "Only the full path proves it works, from invitation to sign-in to file access. Sharing changes can take a few minutes to apply.", 4, "How do you confirm the fix?", new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750") },
                    { new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"), "fabrikam.example isn't on the organization's allowed domains list", new Guid("bb2bb18c-7ecb-0589-c5f3-82f69ef0cb9f"), "An allow list blocks every domain that isn't on it, which is why adatum.example works and fabrikam.example doesn't.", 2, "The site allows New and existing guests and has no domain list. Under Policies > Sharing, the organization limits sharing to allowed domains, and only adatum.example is listed. What's the cause?", new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750") },
                    { new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"), "Try adding the same guest to a different team to see whether the block is tenant-wide or only on this team", new Guid("0e74996b-99eb-69eb-650d-338073472253"), "Guest blocks come either from tenant-wide settings or from one team's label or group setting. One test tells you which layer to inspect.", 1, "What's the quickest way to narrow down the problem?", new Guid("0a9c0e04-1982-c0cb-7c90-748d8bba604c") },
                    { new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"), "Get the data owner's approval for a label that allows guests, then relabel the team", new Guid("afcefab1-8966-a130-15c7-9bb76d63d92b"), "The label records a classification decision, so the owner of the data must approve the change instead of IT working around it.", 3, "The label blocks guests by design, but the business needs the partner in this team. What is the right fix?", new Guid("0a9c0e04-1982-c0cb-7c90-748d8bba604c") },
                    { new Guid("e7de2726-9058-4dab-6026-21eb4d335180"), "Connect-ExchangeOnline -UserPrincipalName admin@contoso.example", new Guid("d3bae316-da15-9833-bd01-f5f769e6eee6"), "The Exchange Online PowerShell V3 module signs in with modern authentication, supports MFA, and loads the Exchange Online cmdlets.", 1, "How do you start the PowerShell session?", new Guid("bf707aa9-af50-2825-fddb-590cb6d999eb") },
                    { new Guid("f2457426-5608-421d-48c7-4da2540fc244"), "The Project Atlas site's sharing settings in the SharePoint admin center, then any domain limits", new Guid("0270171a-9e67-92af-ebbe-95486b07d401"), "Below the organization level, the site setting and domain restrictions are the layers that can block one partner but not another.", 1, "The organization level already allows guests. Where do you look first?", new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750") },
                    { new Guid("f430db22-4555-1b5b-092d-f0e293522887"), "Whether that label's group settings stop owners from adding guests", new Guid("dce58029-463c-b6d4-f87c-6a46bbbe3203"), "Container sensitivity labels can control guest access for a Microsoft 365 group, and they override what the owner can do in that team.", 2, "Other teams can add the guest. Vendor Onboarding has the sensitivity label 'Confidential - Internal'. What do you check?", new Guid("0a9c0e04-1982-c0cb-7c90-748d8bba604c") },
                    { new Guid("fce75af6-65e2-355a-ba86-18691e800a33"), "Add block entries for the domain and URL in the Tenant Allow/Block List", new Guid("926b859f-b559-4c9a-ff26-102e7e81b2a3"), "Block entries stop new mail from the domain, and Safe Links blocks the URL at click time even in messages that get through.", 3, "How do you block further messages and the link itself?", new Guid("342cc552-23d1-4282-8205-22cb79b47a8e") }
                });

            migrationBuilder.InsertData(
                table: "Distractors",
                columns: new[] { "Id", "Explanation", "SequentialStepId", "Text" },
                values: new object[,]
                {
                    { new Guid("035f19a3-62a5-985a-371c-f982dcdbaed4"), "Anyone links remove identity checks for all content, and they still wouldn't satisfy the domain restriction.", new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"), "Change the organization's sharing level to Anyone so links work without sign-in" },
                    { new Guid("04b4bac5-2644-9445-18bf-793e47aa7aa0"), "The search result reflects policy, not a client fault, and reinstalling removes nothing that blocks guests.", new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"), "Reinstall the Teams client on Mei's computer" },
                    { new Guid("0652492f-8da9-54c1-1f7c-c0a320e971cc"), "The invitation failed before any guest account was created, so there's no account to check.", new Guid("f2457426-5608-421d-48c7-4da2540fc244"), "The consultant's guest account in Entra ID, to see whether sign-in is blocked" },
                    { new Guid("06589de0-cfdb-d4e5-94e2-32ca1738f7ab"), "Toggling a setting that's already on changes nothing, and it disrupts existing guests.", new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"), "Turn guest access off and on again in the Teams admin center, then wait for the change to replicate across the tenant before trying again" },
                    { new Guid("0c9b4df9-ce9f-cc8b-c914-c0e7e1d9c27c"), "A DLP block shows a policy tip about sensitive content, and it would affect adatum.example as well.", new Guid("f2457426-5608-421d-48c7-4da2540fc244"), "The data loss prevention policy matches in Microsoft Purview for the files the owners tried to share with the consultant" },
                    { new Guid("15998e8e-6f81-e296-43c8-3c2a2c36abb9"), "This drops the label's other protections and bypasses the data owner's decision.", new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"), "Remove the sensitivity label from the team so the default settings apply" },
                    { new Guid("17bfefb0-2c76-8c72-58d0-99ef99e470da"), "Campaigns rotate sending IPs, and an IP block does nothing about the URL in other messages.", new Guid("fce75af6-65e2-355a-ba86-18691e800a33"), "Block the sending IP address in the connection filter policy" },
                    { new Guid("18d6231f-e9ef-bf72-e9be-a2c3984de383"), "Mail flow rules act on messages in transit, so they can't touch copies that were already delivered.", new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"), "Create a mail flow rule that deletes messages from payroll-portal.example" },
                    { new Guid("1d479b58-b97c-b015-2bac-b8652818070d"), "Those events come from before the change and say nothing about a new attempt.", new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"), "Search the unified audit log for SharingInvitationCreated events from the site's owners over the last week" },
                    { new Guid("204b2563-59a4-b9a3-063d-08d37a7cb6ce"), "Verification happens after an invitation is sent. This one was blocked before it was sent.", new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"), "The consultant's Microsoft account hasn't been verified yet" },
                    { new Guid("20923825-bb61-f636-a595-3ddc8c70591f"), "That allows sign-ins from Portugal for the entire organization, and removing it later is easy to forget.", new Guid("063e942b-84c2-66c3-3815-01f571f03414"), "Add Portugal to the allowed countries location in the policy, then remove it when the trip ends" },
                    { new Guid("20b7f8ce-6954-70cd-c02f-e2e4f8330e78"), "ZAP only acts when a verdict changes, and you can't count on its timing while users are clicking.", new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"), "Wait for zero-hour auto purge to remove them" },
                    { new Guid("21ba96b2-ee8c-8d2b-8083-384022975291"), "That protects against the next message, not against credentials that may already be stolen.", new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"), "Move both users into the Strict preset security policy so future phishing messages are quarantined before they reach the Inbox" },
                    { new Guid("29aef724-c86c-772b-479b-c1dfcf522135"), "Deleting the rule alone leaves the attacker signed in with a valid token, free to create it again.", new Guid("497b1140-0863-3bb1-9706-231416a7b32c"), "Delete the rule with Remove-InboxRule, then ask Grace to watch the Inbox closely for the rest of the week and report anything else that goes missing" },
                    { new Guid("302f453e-5c6e-ca21-e810-a41ec95c3cc4"), "Encryption affects who can open labeled files, not who can be added as a member.", new Guid("f430db22-4555-1b5b-092d-f0e293522887"), "Whether the label applies encryption to the team's files" },
                    { new Guid("3577e16b-f6fe-4477-89ed-ced21d24a952"), "MFA failures return codes such as 500121, and a lockout returns 50053. The log shows a policy block.", new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"), "Luis failed the MFA challenge too many times and the account was locked out" },
                    { new Guid("3a71048b-6910-de7e-2a48-8f6f23ba144a"), "Folder permissions show who can open the Inbox. They don't show rules that move mail out of it.", new Guid("6d2abeae-2397-c7a3-b761-089f45177081"), "Get-MailboxFolderPermission -Identity grace@contoso.example:\\Inbox" },
                    { new Guid("3ec47406-8fae-c087-b3b9-5025533b4308"), "Guests can redeem with a work account, a Microsoft account or a one-time passcode, so the partner's account isn't what's blocking Mei.", new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"), "Ask the partner to create a Microsoft account first" },
                    { new Guid("40af6080-7833-63f5-8cc1-4094791fe289"), "A clean browser session still opens in the guest's home organization.", new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"), "Sign in to Teams on the web in a new private browser window, which clears the cached tenant list from the last session" },
                    { new Guid("426294d2-2f48-c7fd-af81-77baa64d0e6b"), "A block in the partner's tenant fails at redemption, not with your organization's sharing policy message.", new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"), "Fabrikam's own tenant has a cross-tenant access policy that blocks B2B collaboration with outside organizations" },
                    { new Guid("43fc4fbd-bdd1-c307-160f-ee082e97d234"), "Clearing the cache doesn't change policy evaluation, and waiting without testing proves nothing.", new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"), "Ask Luis to clear the browser cache and try again in an hour" },
                    { new Guid("4532c6ff-2bc7-cef8-4556-24e06abb1096"), "The IP can change, and trusting it opens the policy to anyone on that network, including attackers.", new Guid("063e942b-84c2-66c3-3815-01f571f03414"), "Add the hotel's public IP address to a trusted named location" },
                    { new Guid("4b090082-9182-b263-0904-dc9da3c29545"), "The MSOnline module is retired, and it never managed mailboxes or inbox rules.", new Guid("e7de2726-9058-4dab-6026-21eb4d335180"), "Connect-MsolService -Credential (Get-Credential)" },
                    { new Guid("4bb00f17-6fef-44dd-4153-85b79bf8329f"), "People miss warnings, and some will click before they read it.", new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"), "Email all staff a warning, then ask each recipient to delete the message and empty Deleted Items" },
                    { new Guid("4f50d496-ec11-9b18-47d4-9c9960a46a1f"), "The office is inside the allowed countries, so this test can't reproduce Luis's situation.", new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"), "Sign in with a test account from the office network" },
                    { new Guid("5040c497-85b5-4fcd-e7b3-8361cd2639bb"), "Rule reports show which rules matched, not a full list of recipients.", new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"), "Run a mail flow rule report for messages from payroll-portal.example" },
                    { new Guid("5597811f-58bc-f387-45fc-3d8c25c1b450"), "That costs a license, bypasses guest governance, and gives a non-employee a full internal identity.", new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"), "Create an internal account for the consultant and assign it an E3 license" },
                    { new Guid("5e4fcaee-5ede-b2df-2a1d-5e47c5631296"), "The attacker keeps a valid session and can recreate or re-enable the rule at any time.", new Guid("497b1140-0863-3bb1-9706-231416a7b32c"), "Disable the rule with Disable-InboxRule so it can be turned back on later if Grace says it was intentional" },
                    { new Guid("616afcc5-9d02-42f1-8daf-1cb0efb9b4af"), "A saved change proves the policy was edited, not that this user's sign-in will pass.", new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"), "Check the policy's Last modified date and audit log entry to make sure the change was saved correctly" },
                    { new Guid("65c9f5eb-d38f-3cfa-06c1-8eb512967eec"), "Hiding the mailbox from the address book doesn't touch the rule or the attacker's access.", new Guid("497b1140-0863-3bb1-9706-231416a7b32c"), "Set-Mailbox -Identity grace@contoso.example -HiddenFromAddressListsEnabled $true" },
                    { new Guid("685c9c94-e52b-c6ca-dc51-bc79f45387ba"), "Adding guests to a team doesn't need Teams Premium.", new Guid("f430db22-4555-1b5b-092d-f0e293522887"), "Whether Mei has a Teams Premium license" },
                    { new Guid("7ee24511-3d8c-b938-b13b-47a3e27db1e7"), "That confirms the setting, not that the consultant can reach the file.", new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"), "Check that fabrikam.example now appears in the allowed domains list" },
                    { new Guid("87735e36-9a00-8770-6efd-f6704eb9f568"), "No MFA prompt was shown, so the methods were never used. Re-registering removes working methods without touching the block.", new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"), "Require Luis to re-register MFA from the Authentication methods page" },
                    { new Guid("8ad481b2-9100-1e99-8609-7ad225e78444"), "Microsoft Graph PowerShell doesn't load Exchange cmdlets, so Get-InboxRule isn't available in that session.", new Guid("e7de2726-9058-4dab-6026-21eb4d335180"), "Connect-MgGraph -Scopes 'Mail.ReadWrite', then run Get-InboxRule" },
                    { new Guid("909be570-2a63-773b-cda8-1b0ca8be7d78"), "An expired password returns 50055 and prompts for a change. It isn't a Conditional Access result.", new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"), "Luis's password expired while traveling" },
                    { new Guid("91aaae22-83bb-2d39-02f4-da8f0514d4e4"), "Basic authentication for remote PowerShell has been turned off in Exchange Online, so this connection fails.", new Guid("e7de2726-9058-4dab-6026-21eb4d335180"), "New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell-liveid/ -Authentication Basic" },
                    { new Guid("933c7b85-65d8-aa2f-cdd0-b3499318142d"), "That setting is tenant-wide, but other teams can already add the guest.", new Guid("f430db22-4555-1b5b-092d-f0e293522887"), "Whether the Entra external collaboration settings restrict guest invitations to admin roles" },
                    { new Guid("9a857f51-46e5-851c-178d-b668789a1f52"), "The request reached Entra ID and was evaluated, because the sign-in log has an entry with a policy result.", new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"), "The hotel Wi-Fi is blocking traffic to login.microsoftonline.com, so the token request never completes" },
                    { new Guid("9b439ebd-6741-7e0a-5aee-3818b54384fc"), "Mailbox auditing records actions inside the mailbox. It doesn't hold Conditional Access results.", new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"), "Search the Microsoft Purview audit log for Luis's mailbox activity since the last successful sign-in yesterday" },
                    { new Guid("9db73bda-9311-7bed-1dfe-a2fe86d2260c"), "Service health reports Microsoft service issues, not campaigns aimed at your users.", new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"), "Check the Service health page in the Microsoft 365 admin center for an active phishing advisory affecting the tenant" },
                    { new Guid("a1e12b5b-f789-9584-51c2-d462c2912088"), "You just confirmed the site allows New and existing guests.", new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"), "The site's sharing level is more restrictive than the organization's" },
                    { new Guid("a4c552d0-a928-8e53-1f0a-444167405c29"), "The attacker sends from its own domain and passes its own SPF and DKIM. Your DMARC policy only protects your domain.", new Guid("fce75af6-65e2-355a-ba86-18691e800a33"), "Publish a DMARC policy of p=reject for your own domain" },
                    { new Guid("aa467904-f1c9-20aa-84a8-9304f6eb36f5"), "Their copies don't tell you who else received it.", new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"), "Ask the three reporters to forward the message to the service desk" },
                    { new Guid("b4d0c541-9f49-7b1e-a3fb-d60496179991"), "adatum.example already worked, so this test can't prove the fix.", new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"), "Share a test file with an adatum.example user" },
                    { new Guid("b7062ebf-d4aa-9356-31d8-993495ca0df2"), "Outlook doesn't create hidden rules, and the keywords target exactly the wire transfer conversation.", new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"), "Grace created a cleanup rule by mistake and forgot about it" },
                    { new Guid("c508f2ef-6ed6-8f5e-2f09-3ef6d682b0fb"), "Guest access normally works within minutes. The missing step is switching organizations.", new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"), "Wait 24 hours for the guest account to sync to Teams" },
                    { new Guid("c63cd41e-b003-a076-c3dd-2235b4f93e20"), "Impersonation protection guards domains you own or care about. The attacker's domain isn't impersonating anything.", new Guid("fce75af6-65e2-355a-ba86-18691e800a33"), "Add payroll-portal.example to the protected domains for impersonation in the anti-phishing policy" },
                    { new Guid("ce208e75-3f13-bf9f-0226-9773a81a01bc"), "The invitation was already redeemed, so another one changes nothing.", new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"), "Resend the invitation from the Entra admin center" },
                    { new Guid("d8b9749e-5600-8fa5-999d-bbc414a9db9e"), "Teams guest access controls the guest experience in Teams, not sharing from SharePoint.", new Guid("f2457426-5608-421d-48c7-4da2540fc244"), "The guest access settings in the Teams admin center" },
                    { new Guid("d9621d3f-e941-6078-c4b6-829d36f3229e"), "Trace records what transport did, and it already shows Delivered. It can't see what happened to a message after it reached the mailbox.", new Guid("6d2abeae-2397-c7a3-b761-089f45177081"), "Get-MessageTraceV2 -RecipientAddress grace@contoso.example -StartDate (Get-Date).AddDays(-7) -EndDate (Get-Date)" },
                    { new Guid("daf92b36-ad46-f8a5-872e-9dffd9c50712"), "Defender actions show up in message trace and quarantine, not as an inbox rule in the user's mailbox.", new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"), "A Defender for Office 365 policy moved the messages as suspected phishing" },
                    { new Guid("e04a4f39-45b2-44f7-d43a-5d349d7a8c2e"), "This sidesteps the classification and gives an outside person an internal identity and license.", new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"), "Create a member account for the partner in your tenant so the label's guest restriction doesn't apply" },
                    { new Guid("e335cdee-db39-4c02-4d56-4b60c3af02e3"), "Admin roles don't override a label's guest setting, and the role grants far too much access.", new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"), "Assign Mei the Teams Administrator role" },
                    { new Guid("e3da0902-4709-490c-5872-0a82ef5e5817"), "Training is worthwhile later, but it doesn't contain a possible compromise now.", new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"), "Assign both users a phishing awareness training module" },
                    { new Guid("e62736a6-0a05-4276-c4af-b8e08889b446"), "That opens sharing to every external domain to admit one partner.", new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"), "Turn off 'Limit external sharing by domain' at the organization level so the consultant can be invited right away" },
                    { new Guid("e67cbf07-aefa-62a6-6720-1f331ef06ae8"), "This reports item counts and mailbox size, not why particular messages disappear.", new Guid("6d2abeae-2397-c7a3-b761-089f45177081"), "Get-MailboxStatistics -Identity grace@contoso.example" },
                    { new Guid("eb7b3260-87c1-f619-ac80-ac3e20f3c1de"), "Report-only stops enforcement for every user in scope, not just Luis.", new Guid("063e942b-84c2-66c3-3815-01f571f03414"), "Switch the policy to Report-only mode until Luis returns" },
                    { new Guid("ed8b6d3f-35f0-c92c-4572-85fa507316dd"), "Credential phishing steals passwords in the browser, and a malware scan doesn't undo that.", new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"), "Run a full antivirus scan on both users' computers" },
                    { new Guid("f1963da8-6a21-965f-d489-a874bdd777ca"), "A missing license causes service errors after sign-in, not a Conditional Access block during it.", new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"), "Check that Luis still has an E3 license assigned" },
                    { new Guid("f671c797-9d91-e995-69c2-931c894c5665"), "A client sync fault doesn't create a server-side rule with keyword conditions.", new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"), "An Outlook sync problem on Grace's laptop is filing new mail into RSS Subscriptions, so the messages land there automatically" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("035f19a3-62a5-985a-371c-f982dcdbaed4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("04b4bac5-2644-9445-18bf-793e47aa7aa0"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0652492f-8da9-54c1-1f7c-c0a320e971cc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("06589de0-cfdb-d4e5-94e2-32ca1738f7ab"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0c9b4df9-ce9f-cc8b-c914-c0e7e1d9c27c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("15998e8e-6f81-e296-43c8-3c2a2c36abb9"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("17bfefb0-2c76-8c72-58d0-99ef99e470da"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("18d6231f-e9ef-bf72-e9be-a2c3984de383"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1d479b58-b97c-b015-2bac-b8652818070d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("204b2563-59a4-b9a3-063d-08d37a7cb6ce"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("20923825-bb61-f636-a595-3ddc8c70591f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("20b7f8ce-6954-70cd-c02f-e2e4f8330e78"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("21ba96b2-ee8c-8d2b-8083-384022975291"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("29aef724-c86c-772b-479b-c1dfcf522135"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("302f453e-5c6e-ca21-e810-a41ec95c3cc4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3577e16b-f6fe-4477-89ed-ced21d24a952"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3a71048b-6910-de7e-2a48-8f6f23ba144a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3ec47406-8fae-c087-b3b9-5025533b4308"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("40af6080-7833-63f5-8cc1-4094791fe289"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("426294d2-2f48-c7fd-af81-77baa64d0e6b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("43fc4fbd-bdd1-c307-160f-ee082e97d234"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4532c6ff-2bc7-cef8-4556-24e06abb1096"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4b090082-9182-b263-0904-dc9da3c29545"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4bb00f17-6fef-44dd-4153-85b79bf8329f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4f50d496-ec11-9b18-47d4-9c9960a46a1f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5040c497-85b5-4fcd-e7b3-8361cd2639bb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5597811f-58bc-f387-45fc-3d8c25c1b450"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5e4fcaee-5ede-b2df-2a1d-5e47c5631296"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("616afcc5-9d02-42f1-8daf-1cb0efb9b4af"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("65c9f5eb-d38f-3cfa-06c1-8eb512967eec"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("685c9c94-e52b-c6ca-dc51-bc79f45387ba"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7ee24511-3d8c-b938-b13b-47a3e27db1e7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("87735e36-9a00-8770-6efd-f6704eb9f568"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8ad481b2-9100-1e99-8609-7ad225e78444"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("909be570-2a63-773b-cda8-1b0ca8be7d78"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("91aaae22-83bb-2d39-02f4-da8f0514d4e4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("933c7b85-65d8-aa2f-cdd0-b3499318142d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9a857f51-46e5-851c-178d-b668789a1f52"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9b439ebd-6741-7e0a-5aee-3818b54384fc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9db73bda-9311-7bed-1dfe-a2fe86d2260c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a1e12b5b-f789-9584-51c2-d462c2912088"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a4c552d0-a928-8e53-1f0a-444167405c29"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("aa467904-f1c9-20aa-84a8-9304f6eb36f5"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b4d0c541-9f49-7b1e-a3fb-d60496179991"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b7062ebf-d4aa-9356-31d8-993495ca0df2"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c508f2ef-6ed6-8f5e-2f09-3ef6d682b0fb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c63cd41e-b003-a076-c3dd-2235b4f93e20"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ce208e75-3f13-bf9f-0226-9773a81a01bc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d8b9749e-5600-8fa5-999d-bbc414a9db9e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d9621d3f-e941-6078-c4b6-829d36f3229e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("daf92b36-ad46-f8a5-872e-9dffd9c50712"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e04a4f39-45b2-44f7-d43a-5d349d7a8c2e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e335cdee-db39-4c02-4d56-4b60c3af02e3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e3da0902-4709-490c-5872-0a82ef5e5817"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e62736a6-0a05-4276-c4af-b8e08889b446"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e67cbf07-aefa-62a6-6720-1f331ef06ae8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("eb7b3260-87c1-f619-ac80-ac3e20f3c1de"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ed8b6d3f-35f0-c92c-4572-85fa507316dd"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f1963da8-6a21-965f-d489-a874bdd777ca"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f671c797-9d91-e995-69c2-931c894c5665"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("063e942b-84c2-66c3-3815-01f571f03414"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("497b1140-0863-3bb1-9706-231416a7b32c"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d2abeae-2397-c7a3-b761-089f45177081"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e7de2726-9058-4dab-6026-21eb4d335180"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f2457426-5608-421d-48c7-4da2540fc244"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f430db22-4555-1b5b-092d-f0e293522887"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("fce75af6-65e2-355a-ba86-18691e800a33"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("0a9c0e04-1982-c0cb-7c90-748d8bba604c"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("342cc552-23d1-4282-8205-22cb79b47a8e"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("bf707aa9-af50-2825-fddb-590cb6d999eb"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9"));
        }
    }
}
