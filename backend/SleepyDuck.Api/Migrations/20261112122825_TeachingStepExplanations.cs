using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class TeachingStepExplanations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("015dd9c5-6161-0c58-b668-a5b11b501069"),
                column: "Explanation",
                value: "A license such as E3 is a bundle of service plans: Exchange Online, SharePoint, Teams, the Office apps, Viva Engage and others. In the wizard, or later on the user's Licenses and apps tab, the Apps list lets you clear individual plans. Turning a plan off removes Dana's entitlement to that service and leaves the rest of E3 working. One thing to know is that OneDrive depends on the SharePoint plan, so turning SharePoint off removes OneDrive too.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"),
                column: "Explanation",
                value: "Clicking past the warning means the user reached the fake sign-in page and may have typed a password. Assume they did. Reset the password, revoke sessions so stolen tokens stop working, and review the sign-in logs for sign-ins from unfamiliar locations. Also check for new inbox rules, forwarding and newly registered MFA methods, which attackers add to keep access.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03fe44d1-045b-dd43-5c2a-8f28a9bf0e88"),
                column: "Explanation",
                value: "In the Exchange admin center, open Recipients > Groups, pick the Distribution list tab, select the group, and edit Manage delegates in its settings. There you add Aisha and choose the Send As permission. The permission is stored on the group, which is why it's set there and not on Aisha's account.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"),
                column: "Explanation",
                value: "dig's answer comes from the DNS server, and it's the new address, so the record is correct. dscacheutil's answer comes from the Mac's own resolver, and it's the old one. The gap between them is local to this Mac: a cached result, or something that overrides DNS. The Windows laptops already suggested the server side was fine, and this confirms it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("063e942b-84c2-66c3-3815-01f571f03414"),
                column: "Explanation",
                value: "Policies have Include and Exclude lists, and Exclude wins. Putting Luis in a group that the policy excludes lets only him through, and only while he's in the group. Set a removal date, or use an access review, so the exception doesn't become permanent. The other options all change the policy for everybody.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0746394d-1ec2-c78f-4b8e-ed734c66af0f"),
                column: "Explanation",
                value: "With group-based licensing you assign the license to a group once, and Entra ID gives it to every member. A new hire who is added to the group gets E3 automatically, and someone who is removed loses it and the license returns to the pool. It needs a security group or a Microsoft 365 group, and Entra ID P1, which E3 includes. If membership is dynamic, based on the department attribute, even adding people to the group is automatic.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"),
                column: "Explanation",
                value: "A normal cleanup keeps what's needed to uninstall recent updates. /ResetBase removes every superseded version, which frees more space but makes the installed updates permanent. If one of them later turns out to cause a problem, it can't be rolled back. Updates installed afterwards can still be uninstalled. On a production server, use it only when the installed updates are known to be stable.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("08877fab-aa11-4280-5064-4ea65ce68431"),
                column: "Explanation",
                value: "Put the results together. The service works locally, and the port can't be reached from a PC on the same subnet. Same subnet means no router or network firewall sits in between. The only thing left between that PC and the listener is Windows Firewall on the server itself, and a hardening change the night before is a likely reason for a disabled rule.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"),
                column: "Explanation",
                value: "A full disk needs fast action, but deleting the wrong thing on a system drive can stop the server from booting. Measure first. A tool such as WinDirStat or TreeSize, or a PowerShell loop that totals folder sizes, shows the largest folders in minutes. That tells you what to remove, and it gives you the root cause for the ticket.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"),
                column: "Explanation",
                value: "A Conditional Access policy is an if-then rule: if these users, apps and conditions match, then block, or grant access with requirements such as MFA. This policy's condition is any location outside the 'allowed countries' named location, and its control is Block. Entra works out the country from the public IP address of the sign-in. Luis's IP is Portuguese, so the condition matched. A block ends the sign-in at once, which is why he was never asked for MFA.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"),
                column: "Explanation",
                value: "Test-ComputerSecureChannel checks whether the PC can authenticate to a domain controller with its machine account password. It returns True for a healthy channel and False for a broken one, and -Verbose adds the reason. nltest /sc_verify with the domain name does the same check. It turns the error message into a confirmed diagnosis before you change anything.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1321e4a7-c19a-7752-68a2-8d1f4257a090"),
                column: "Explanation",
                value: "Policies are scoped to users, groups or domains, and a custom policy takes precedence over the Default one. A custom outbound policy with forwarding set to On, applied only to Marco, allows the approved exception, and everyone else keeps the blocked default. Record who approved it, because each mailbox that can forward externally is one an attacker could use to leak mail.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("134765e8-456a-7523-7bb0-d74aabba779b"),
                column: "Explanation",
                value: "Group licensing is processed in the background, one user at a time, and a user who fails doesn't stop the others. The group's Licenses page shows how many members are in an error state, and opening it lists each user with a reason, such as a missing usage location, not enough licenses, or conflicting service plans. The same information is in the Entra audit logs.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"),
                column: "Explanation",
                value: "macOS keeps its DNS cache in the mDNSResponder process. dscacheutil -flushcache clears the Directory Service cache, and killall -HUP mDNSResponder sends the hang-up signal, which tells the daemon to throw away its cache and carry on running. Both need sudo. The command prints nothing when it works, so check by running dscacheutil -q host again.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1823c892-3611-380b-80d0-0441fa9616af"),
                column: "Explanation",
                value: "Security Filtering grants two permissions together, Read and Apply group policy. The Delegation tab can grant Read by itself. Adding Domain Computers, or Authenticated Users, with Read lets every PC fetch the GPO. Only Sales-Users has Apply, so the drive still maps for the sales team alone.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"),
                column: "Explanation",
                value: "The certificate is only checked during an authentication. Macs that were already connected kept their sessions after the renewal. The wireless controller or RADIUS server sets a session timeout, here 8 hours, and when it expires the client must authenticate again. That was the first time each Mac saw the new certificate, and that's when it failed. It's a pattern worth knowing: a change can break things at the next reauthentication, not at once.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"),
                column: "Explanation",
                value: "The permissions are right. The share lets any authenticated user through with Full Control, and NTFS gives Modify to Finance-RW, so NTFS decides and members get Modify. Tomas is a member in Active Directory, but whoami /groups shows his current access token, and the group isn't in it. Windows builds the token once, at sign-in, from the groups he had at that moment. He signed in this morning and was added an hour ago. The file server checks the token, finds no Finance-RW, and denies access.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("22105984-345f-f942-460c-ef2a7966ae39"),
                column: "Explanation",
                value: "An ISP acts fastest on evidence. Send MTR reports with timestamps from both directions, which show loss starting at its gateway, along with the clean interface counters that rule out your equipment. That lets the engineer go straight to the handoff. A vague report of slow internet starts with the first-line script.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"),
                column: "Explanation",
                value: "Take the biggest and safest item first. IIS writes a log file per site per day under C:\\inetpub\\logs\\LogFiles and never deletes them, so they grow without limit. They're only records, so removing old ones can't harm Windows or the site. Ask the application owner or security how long logs must be kept, archive what's needed to another volume, and delete the rest. That frees up to 38 GB.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"),
                column: "Explanation",
                value: "Over UDP, SIP resends a request that gets no response, doubling the wait each time: 0.5, 1, 2 and 4 seconds. A phone that receives an INVITE answers almost at once with 100 Trying, then 180 Ringing. Neither appears, so the INVITE never arrived, or the answers were lost. After 32 seconds, which is Timer B, the PBX gives up and returns 408 to the caller.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"),
                column: "Explanation",
                value: "What If is a tool on the Conditional Access page. You pick a user, an app and conditions such as country or IP address, and it lists which policies would apply and which wouldn't. Running it for Luis from Portugal should show the block policy among the policies that will not apply, because of the exclusion. That proves the change before he tries. His next real sign-in in the sign-in logs is the final confirmation.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"),
                column: "Explanation",
                value: "Replication between sites runs on a schedule, so force it with repadmin /syncall /AdeP, or with Replicate Now in Active Directory Sites and Services. Then run repadmin /replsummary again. BR-DC02 should show 0 failures and a largest delta of minutes, not two days. As a final check, an account created this morning should exist on BR-DC02 and be able to sign in at the branch.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("381ec257-5b7d-2896-21d5-ba29c9e460f2"),
                column: "Explanation",
                value: "Two things commonly go wrong straight after the grant. The permission can take up to an hour to apply across Exchange Online. And Outlook keeps sending from Aisha's own address until she shows the From field and chooses the support address. If Outlook's cached entry for the address bounces, typing it out in full or using Outlook on the web avoids it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("399d9d86-b7dd-b66a-6074-15cddd0bf5bc"),
                column: "Explanation",
                value: "Automatic external forwarding is governed by outbound spam filter policies, under Email & collaboration > Policies & rules > Threat policies > Anti-spam in the Defender portal. The setting has three values: Automatic - System-controlled, On, and Off. The default, Automatic, behaves as Off, which is why a rule that a user is able to create still gets blocked. It's off by default because attackers who take over a mailbox often add a forwarding rule to copy mail out.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"),
                column: "Explanation",
                value: "Repeat the tests that failed. Test-NetConnection on port 3389 should return True, and an administrator should be able to open a session from their own PC. Then find what disabled the rule. If the hardening script or GPO still does it, the rule goes off again at the next run or policy refresh. Fix it at the source, with an exception that allows RDP from the admin network.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"),
                column: "Explanation",
                value: "Every sign-in attempt is logged in Entra ID under Sign-in logs, including failed ones. The failed entry gives the error code, the IP address and the location it mapped to, the app, and the device. Its Conditional Access tab lists every policy that was evaluated and whether each one succeeded, failed or didn't apply. That names the policy that blocked Luis, so you don't have to guess.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3d623bd9-091b-bf4a-8d79-7c3b087fb6ee"),
                column: "Explanation",
                value: "Two small changes fix this. Move the disclaimer rule to a higher priority than 'Vendor Bypass', so it runs first. Or clear 'Stop processing more rules' on 'Vendor Bypass', so later rules still run. Find out why the bypass rule exists before choosing, and leave whatever else it does alone.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"),
                column: "Explanation",
                value: "Check both sets on the server. Share permissions are on the folder's Sharing tab under Advanced Sharing > Permissions, or from Get-SmbShareAccess -Name Finance. NTFS permissions are on the Security tab. Under Advanced, the Effective Access tab works out the NTFS result for a user you pick, adding up all the groups they belong to and applying any Deny entries. It reads his membership from Active Directory, so it shows what he should get, which can differ from what his current session has.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"),
                column: "Explanation",
                value: "The failure only appeared at reauthentication, so a first connection doesn't prove the fix. Confirm the new profile is on the Macs, then watch the same Macs past a full 8-hour session. On NPS, event 6272 means access was granted and 6273 means it was denied. A 6272 for a Mac's reauthentication after the timeout, with no 6273, is the proof.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"),
                column: "Explanation",
                value: "Add the approved domain to the allow list under Policies > Sharing in the SharePoint admin center. That admits Fabrikam and nobody else. Sharing with a new guest also creates a Microsoft Entra B2B guest account, and Entra has its own allow or deny list under External collaboration settings. If that list is in use and Fabrikam is missing, the invitation fails at that second gate, so update both.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("46baf750-afaa-a6df-25f8-be25c4030396"),
                column: "Explanation",
                value: "Replication is pull-based. BR-DC02 connects to HQ-DC01 over RPC and asks for changes. Error 1722, 'The RPC server is unavailable', means that connection couldn't be made. The causes are a name that doesn't resolve, a blocked port (TCP 135 and the dynamic RPC range), or a source server that is down. HQ-DC01 is serving headquarters, so look at DNS and the path between the sites.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"),
                column: "Explanation",
                value: "For a GPO to apply to a user, three things must hold. It's linked to an OU that contains the user, the user has Read and Apply group policy permission on it, and the PC can read it. The report shows the link, the OU and the group membership are right. Access denied is a permissions message, and the Delegation tab shows the GPO's full permission list.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"),
                column: "Explanation",
                value: "Two things can leave a client without an offer. Either the server can't be reached, or it has nothing to give. Clients that already hold a lease are renewing successfully, so the server is reachable. Get-DhcpServerv4ScopeStatistics, or the scope's statistics in the DHCP console, shows addresses in use, addresses free and the percentage used, which confirms an empty pool in one command.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("497b1140-0863-3bb1-9706-231416a7b32c"),
                column: "Explanation",
                value: "Containment has two halves. Remove-InboxRule and Set-Mailbox with -ForwardingSmtpAddress $null undo what the attacker set up. Resetting the password and running Revoke-MgUserSignInSession remove the attacker's access, because revoking invalidates the refresh tokens that would otherwise keep working after a password change. Doing only the first half lets the attacker create the rule again. After that, review the sign-in logs and MFA methods, and warn Finance about the wire transfer.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"),
                column: "Explanation",
                value: "802.1X has three parts. The Mac is the supplicant, the access point is the authenticator, and the RADIUS server decides. To see why it fails you need both ends. On the Mac, the eapolclient process writes the EAP conversation to the system log, which you can read with the log show command. The RADIUS server's log gives its reason for each accept or reject. Match the two at the times of the drops.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"),
                column: "Explanation",
                value: "Each line of MTR is that router's reply to probes addressed to it, and routers give those replies a low priority and often rate limit them. Traffic passing through a router is handled separately. If hop 5 were dropping forwarded packets, hops 6 to 9 would show loss as well, because their probes pass through it. They show 0%, so hop 5 is forwarding fine. The rule is that loss only counts when it continues to the final hop.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4fbf285b-f7f3-0850-2261-611d7f98d9cc"),
                column: "Explanation",
                value: "Releasing fixes today's messages only. A submission marked as clean sends the message to Microsoft for analysis, and it can create an allow entry in the Tenant Allow/Block List at the same time. That entry covers this sender only and expires, by default 45 days after it was last used. The broad shortcuts are riskier, because they skip filtering for anyone who forges the supplier's address.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"),
                column: "Explanation",
                value: "sngrep runs on the PBX and draws each call as a ladder diagram, which shows every SIP message between the PBX and the phone in order, with timing. tcpdump can save the same traffic for Wireshark. The ladder shows whether the PBX sent the INVITE, where it sent it, and whether anything came back.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"),
                column: "Explanation",
                value: "A domain controller finds its partner through DNS, using a CNAME record for the partner's GUID in the _msdcs zone and then its host record. dcdiag /test:dns checks that the DC's DNS client settings are sound and that these records can be resolved and registered. DNS is the most common cause of 1722 and quick to test, so do it before looking at firewalls.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"),
                columns: new[] { "CorrectAction", "Explanation", "Prompt" },
                values: new object[] { "Share permissions first, then NTFS permissions, and he gets only what both sets allow", "A network connection to \\\\FILE-SRV01\\Finance passes two gates. First the server checks the share permissions on the Finance share, which only apply to access over the network. Then it checks the NTFS permissions on the folder itself, which apply to every kind of access. Windows works out his total in each set from all his groups, and the more restrictive of the two totals is what he gets. Full Control on the share with Read in NTFS gives Read, and Read on the share with Modify in NTFS also gives Read. For this ticket it means a block in either set is enough to cause 'Access is denied', so you have to look at both.", "Tomas opens \\\\FILE-SRV01\\Finance over the network. Which permissions does the server check, and how do they combine?" });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"),
                column: "Explanation",
                value: "repadmin /replsummary contacts every domain controller and prints one line each, as a source and as a destination: how many replication attempts failed, the longest time since a success, and the error code. It shows in seconds which DC is behind and for how long. repadmin /showrepl on that DC then gives the detail for each partition and partner.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5fe20ef1-1f7b-cd82-6161-354cf1b74257"),
                column: "Explanation",
                value: "Don't reason from the rule list. Look at one real message. Run a message trace for a vendor message, open it, and read the events. The trace lists each mail flow rule that the message matched. If the disclaimer rule isn't in that list, the question becomes why it didn't run, and whichever rule did match is the first suspect.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("601ffe8b-0de5-9e57-9875-6f9be645da90"),
                column: "Explanation",
                value: "A trace needs the sender, the recipient and a date range. Using both addresses returns only the supplier's messages to Priya, and the window has to reach back to the first missing invoice. Searches of the last 10 days come back within moments, and older ones run as a downloadable report, up to 90 days back. Each result then shows a status and the events behind it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"),
                column: "Explanation",
                value: "A guest account lives in your tenant, Contoso, and not in the guest's home tenant. Teams opens in the home organization by default and shows only its teams. The guest has to select their profile picture and switch to Contoso in the list of organizations. The Vendor Onboarding team appears there.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"),
                column: "Explanation",
                value: "Each detail fits business email compromise. A name like '.' is hard to notice, the rule is hidden from Outlook, the keywords match the wire transfer, and RSS Subscriptions is a folder nobody opens. Marking the mail as read hides the unread count too. The purpose is to keep Grace from seeing the bank's replies while the attacker answers them, usually to change the payment details. This is now a security incident, not a mail flow ticket.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"),
                column: "Explanation",
                value: "The records that domain controllers need exist only on the internal DNS servers that host the Active Directory zones. 8.8.8.8 is a public resolver. It answers quickly that those names don't exist, and Windows accepts that answer and doesn't try the alternate server. Set the preferred DNS back to an internal DNS server, then run ipconfig /flushdns to clear the cached failures. Running ipconfig /registerdns and restarting Netlogon re-registers the DC's own records.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d2abeae-2397-c7a3-b761-089f45177081"),
                column: "Explanation",
                value: "Message trace only covers transport, and Delivered means the message reached the mailbox. Whatever happened next happened inside the mailbox, and inbox rules are the usual cause. Get-InboxRule lists them, and -IncludeHidden adds the rules that Outlook doesn't show, which attackers create on purpose. The Description property reads out each rule's conditions and actions in plain words. Also run Get-Mailbox and look at ForwardingSmtpAddress, the other common place to hide a redirect.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"),
                column: "Explanation",
                value: "In SDP, the c= line is the address where that side wants to receive audio. The phone put its private LAN address there, 192.168.10.25. The provider sends its RTP to that address, and private addresses can't be routed on the internet, so those packets go nowhere. Audio from the phone still gets out, because outbound packets pass through NAT normally. That's why the staff hear nothing and the other party hears them.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"),
                column: "Explanation",
                value: "A lease reserves an address for its whole duration, even after the device has left the building. A /24 gives 254 usable addresses, and the Windows default lease is 8 days. During a week-long conference, each visitor's phone holds an address for 8 days after one visit. A phone that uses a private Wi-Fi address can appear as a new client on a later day and take another one. The demand is real, and the leases are held far too long for it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"),
                column: "Explanation",
                value: "A VoIP call uses two protocols. SIP is the signaling that makes the phone ring and connects the call. RTP carries the audio, as a separate UDP stream in each direction, on different ports. These calls ring and connect, so SIP works. One direction of audio is missing, so one RTP stream isn't arriving. That narrows the fault to the media path, and the new firewall is the obvious suspect.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"),
                column: "Explanation",
                value: "Threat Explorer, under Email & collaboration > Explorer in the Defender portal, searches all mail in the tenant. Filter on the sender address, the subject or the URL, and it returns every copy with its recipient, where it was delivered (Inbox, Junk or quarantine), and whether anyone clicked the link. It comes with Defender for Office 365 Plan 2. Three reports usually mean many more recipients, and this is how you find the full scope.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"),
                column: "Explanation",
                value: "This is the pattern of real loss. It begins at one hop, and every hop after it shows about the same figure, right through to the destination. The first hop with loss is hop 3, so the fault is at hop 3 or on the link between hop 2 and hop 3. Hop 2 is your firewall and hop 3 is the ISP's gateway, so that link is the handoff to the ISP.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72bf031d-2d13-1a34-549a-b71b4faffb68"),
                column: "Explanation",
                value: "Quarantined means Exchange Online Protection or Defender for Office 365 judged the message unsafe and held it, so it never reached any mailbox folder and the sender got no bounce. In the Defender portal, go to Email & collaboration > Review > Quarantine. Check the sender, the authentication results and a preview before releasing, because an invoice is exactly what a phishing email imitates.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"),
                column: "Explanation",
                value: "From the Explorer results you select the messages and choose Take action. A soft delete moves each copy to the Recoverable Items folder, where users can't see it but an admin can still recover it. The Action center records which mailboxes were cleaned and which failed. It's faster and more complete than asking 140 people to delete a message.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("7baf03c7-e445-9275-d136-72e655c7b8f2"),
                column: "Explanation",
                value: "Internal mail works and nothing is in Junk, so Outlook and the mailbox are fine and the message was stopped before delivery. Message trace is Exchange Online's record of every message it handled. It shows whether a message was received, and whether it was Delivered, Quarantined, Failed or still Pending. If the trace finds nothing at all, the message never reached Microsoft 365 and the problem is on the sender's side.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("801fb6ed-6c3f-95a7-9828-690e0b11d9ae"),
                column: "Explanation",
                value: "Verify the same way you diagnosed it. Wait for the change to apply, get a real message from vendor.example, and check that it carries the disclaimer. Then open its message trace and confirm that the disclaimer rule now appears among the matched rules. A test from inside the company wouldn't match the external sender condition.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"),
                column: "Explanation",
                value: "Since the MS16-072 update in 2016, Windows downloads user policy using the computer account, where it used to use the user's. So the computer needs Read on the GPO. Normally it has that through Authenticated Users, which includes computer accounts. Removing that group from Security Filtering took away Read as well as Apply. Nothing in this list covers the sales PCs, so they can't read the GPO, and it's skipped.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("81b4863b-3305-e700-519f-7b935733ab79"),
                column: "Explanation",
                value: "The machine password changes every 30 days, and the checkpoint was ten weeks old. Since then the PC changed its password at least twice, and Active Directory stores the newest one. Reverting brought back the PC's copy from ten weeks ago, which the domain no longer accepts. Being switched off isn't the cause, because Active Directory never expires a machine password. Restoring old state is.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("827bc225-d24a-2406-a175-96abdf975124"),
                column: "Explanation",
                value: "On a Mac there are two ways to look up a name, and they can disagree. dig sends a query straight to the DNS server and shows that server's answer. dscacheutil -q host goes through the macOS system resolver, mDNSResponder, which is what Safari and other apps use, and it includes the cache and local overrides. Running both tells you whether DNS is wrong or only this Mac is.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("838d0d9a-847e-2dbd-3839-3b5ba965d837"),
                column: "Explanation",
                value: "The error names the missing property, so fix that property and nothing else. Set the usage location on the three users, and Entra ID normally retries the assignment by itself. The Reprocess button on the group's Licenses page forces it straight away. To prevent it, set the usage location when users are created, or sync it from HR data.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"),
                column: "Explanation",
                value: "Test the whole path the consultant will use. An owner shares the file again, the consultant receives the invitation, signs in or enters a one-time passcode to redeem it, and opens the file. Each of those steps can fail separately, so seeing the domain in the list isn't enough. Allow a few minutes after the change before testing.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"),
                column: "Explanation",
                value: "The system resolver has sources other than DNS, and a flush doesn't touch them. The file /etc/hosts is read first, so a line for intranet.corp.example there, perhaps left from testing, overrides DNS permanently. scutil --dns lists every resolver the Mac uses, including ones a VPN adds for specific domains, which may point at a DNS server that still has the old record.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"),
                column: "Explanation",
                value: "'Limit external sharing by domain' has two modes. In allow mode, only the listed domains can be shared with, and every other domain is blocked. In block mode, only the listed domains are refused. This tenant uses allow mode with a single entry, adatum.example, so fabrikam.example is refused with the policy message the owners saw. The site wasn't the cause, since it has no list of its own and already allows guests.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"),
                column: "Explanation",
                value: "Use the same model as before: the share stays broad, and NTFS does the real control. The share gives Authenticated Users Full Control, so the NTFS entry decides what auditors get. Read & execute lets them open and list files but not change them. Grant it to a group, not to named people, so access follows membership as auditors come and go.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("941e7c17-066e-1729-ea54-2d58af65c26d"),
                column: "Explanation",
                value: "Active users > Add a user creates the Entra ID account and walks through the basics, then the usage location and license, then optional roles and profile details, in one pass. The mailbox and other services are provisioned once the license is assigned. For many users at once you'd use a CSV import or PowerShell, but for one new hire the wizard is the standard route.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("94417086-b2e1-926e-f906-10354c29eed4"),
                column: "Explanation",
                value: "Fix the cause, or the disk fills again. Set a retention for the IIS logs, for example a scheduled task that deletes files older than 30 or 90 days, or move the log directory to a data volume in the IIS logging settings. Then set monitoring to warn at around 20% free, so there's time to act before updates and the application start failing.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("981d38df-be64-d403-71f2-931cdbd8145e"),
                column: "Explanation",
                value: "A new sign-in makes Windows build a new access token and get new Kerberos tickets, and those include Finance-RW. Nothing on the server needs to change, because the permissions were already correct. Afterwards, whoami /groups should list Finance-RW, and the share should open. Running klist purge can refresh the tickets without a sign-out, but signing out and in is the reliable answer.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"),
                column: "Explanation",
                value: "The stored password is only used when the service starts. The service started before the rotation and stayed signed in, so it kept running on its existing session. Sunday's patching restart stopped it, and the next start was the first time the old stored password was tried. This gap between the change and the failure is why password rotations cause outages days later.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"),
                column: "Explanation",
                value: "A packet capture shows what's on the wire. In Wireshark, Telephony > VoIP Calls lists each call, and Flow Sequence draws the SIP messages. Inside the INVITE and the 200 OK is the SDP, where each side states the IP address and port it wants audio sent to. Telephony > RTP > RTP Streams shows which audio streams exist. With those you can see whether a stream was never sent or was sent to the wrong place.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"),
                column: "Explanation",
                value: "Every domain-joined PC has a computer account in Active Directory with its own password, which the PC changes by itself every 30 days. The PC uses that password to open a secure channel to a domain controller, and it needs the channel to check users' sign-ins. If the password stored on the PC doesn't match the one in Active Directory, the domain controller refuses the channel. Every domain user on that PC then gets this error.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"),
                column: "Explanation",
                value: "A local account is stored on the PC itself, in the SAM database, and it's verified there without any domain controller. That's why it still works when the secure channel is down. Use the built-in Administrator or a local admin managed by LAPS, signing in as .\\Administrator so Windows checks the local database.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"),
                column: "Explanation",
                value: "Reproduce the conditions of the failure. The fault needs the NAT mapping to expire, so leave the phone idle for several minutes, well past 60 seconds, then call it. The ladder should now show the INVITE answered by 100 Trying and 180 Ringing, and the phone should ring. A test straight after a reboot would have passed before the fix.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"),
                column: "Explanation",
                value: "The quick fix has three parts. Shorten the lease on this scope to a few hours, so addresses return soon after a visitor leaves. That only takes effect when a client renews or gets a new lease. Delete the stale leases to free addresses now. Turn on conflict detection, which makes the server ping an address before offering it, in case a device is still using one you deleted.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"),
                column: "Explanation",
                value: "Start by finding out which side of the gateway the loss is on. Ping the default gateway to test the LAN, then an external address to test the path beyond it. Use around 100 packets, for example ping -c 100, because the result is a percentage and a few packets can easily miss occasional loss. A clean gateway with loss outside points to the WAN.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"),
                column: "Explanation",
                value: "Test the path that was failing: a sales user on a sales PC. gpupdate /force refreshes policy, and gpresult /r should then list Sales Drive Map under the user's applied GPOs, with the drive showing in File Explorer. A user outside Sales-Users shouldn't get the drive, which confirms the filtering still works.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a36f3e7c-c851-b3ce-5b5f-8261ccd91317"),
                column: "Explanation",
                value: "Not every Microsoft 365 service is available in every country, so the usage location tells Microsoft which services it can provide to this user. Assigning a license without one fails. It's a property of each user, and a missing value is the most common reason a license assignment fails, including with group-based licensing.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"),
                column: "Explanation",
                value: "A service that runs as a user account stores that account's password in its own configuration on the server. At each start, Windows signs the account in with the stored password. The security team changed the password in Active Directory last week, and nobody updated the copy stored in the service. Event 7038 says exactly that: the currently configured password is incorrect.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a7ba522e-0217-6b71-33a8-76cb23de155d"),
                column: "Explanation",
                value: "A Microsoft 365 Group rejects mail from senders outside the organization by default, so the partners get a bounce. The fix is the group setting 'Allow external senders to email this group', on the group's Settings tab in the admin center. In PowerShell it's Set-UnifiedGroup with -RequireSenderAuthenticationEnabled $false. Distribution lists have the same default and the same kind of switch.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a80dc289-87e8-7b23-fa54-93d9b8b1f698"),
                column: "Explanation",
                value: "Add-RecipientPermission is the Exchange Online cmdlet for Send As. -Identity is the address being sent as, -Trustee is the person who gets the right, and -AccessRights SendAs is the right. Check it with Get-RecipientPermission -Identity support@contoso.example. Send on Behalf uses a different cmdlet and parameter, Set-DistributionGroup -GrantSendOnBehalfTo.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"),
                column: "Explanation",
                value: "An account locks when bad password attempts reach the lockout threshold. Dana isn't at her desk when it happens and it repeats every 20 minutes, so a device or program is sending her old password on a schedule. Unlocking resets the counter, and the next few attempts lock it again. The real job is to find the source, and the logs can name it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"),
                column: "Explanation",
                value: "The lease change manages the pool better, but it doesn't make it bigger. Widening to a /23 doubles the subnet to 510 usable addresses, and the mask must change on the gateway, the scope and any static devices together. A separate guest VLAN with its own scope gives visitors their own pool and also keeps them off the staff network, which is the better design.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"),
                column: "Explanation",
                value: "The suspect link has two ends, and one of them is yours. Check the firewall's WAN interface counters for CRC errors, a speed or duplex mismatch, and utilization near the limit, since any of those would make it your fault to fix. Traffic often returns by a different route, and MTR only shows the outbound path, so a second MTR run from the far side toward you covers the return path.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b70a712e-865b-04bf-a392-cb31612c6602"),
                column: "Explanation",
                value: "A group managed service account, or gMSA, is an Active Directory account built for services. Active Directory generates and rotates its password automatically, every 30 days by default, and the servers allowed to use it retrieve the password themselves. Nobody types it, and no stored copy goes stale. It needs a KDS root key in the domain and a service that supports running as a gMSA.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b731b4cb-9f14-159f-5c15-56786d65694d"),
                column: "Explanation",
                value: "Owners can add and remove members and change the group's settings, so the team runs itself without tickets to IT. With two owners, the group isn't left ownerless when one of them leaves the company. A group with no owner can only be managed by an admin.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"),
                column: "Explanation",
                value: "A client gets its address by broadcasting a DHCP Discover and waiting for an Offer. If no offer comes, Windows and macOS give themselves an address in 169.254.0.0/16, called APIPA or link-local. It only works on the local segment and has no gateway. So this address always means the client asked for a lease and got no answer.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"),
                column: "Explanation",
                value: "Test from the inside outwards. mstsc to localhost on the server uses the RDP service without crossing the network or the firewall, so if it works, the service is healthy. Then, from another PC, Test-NetConnection FILE-SRV03 -Port 3389 tries a TCP connection to the RDP port. TcpTestSucceeded: True means the port is reachable, and False means something is blocking it. Ping can't answer this, because it uses ICMP and not TCP 3389.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c4e4078b-5ba5-d86b-f7e4-2d13793e498a"),
                column: "Explanation",
                value: "Verify what was actually saved, before the user arrives. The user's Licenses and apps tab shows the assigned license and the state of each service plan, so you can see E3 assigned with SharePoint and Viva Engage cleared. Other admin pages show tenant totals or a single service, which doesn't answer the HR requirement.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"),
                column: "Explanation",
                value: "Several settings must all allow a guest: Teams guest access, the Entra external collaboration settings, the Microsoft 365 Groups guest settings, and any sensitivity label on the team. The first three are tenant-wide, and a label affects one team. Trying the same guest in another team separates these in one test. If it fails everywhere, look at a tenant setting. If it fails only here, look at this team.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"),
                column: "Explanation",
                value: "The Macs' trust settings come from the MDM profile, so that's where the fix goes. Update the Wi-Fi payload with the new root and intermediate CA certificates and the RADIUS server's name under trusted server names, and push it. Every Mac picks it up without user action. Validation stays on, and validation is what stops a fake access point with the same SSID from collecting passwords.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"),
                column: "Explanation",
                value: "The far end needs a public address and port to send audio to. STUN lets the phone learn its public address and put it in the SDP. Or the provider's SBC can use symmetric RTP, where it ignores the SDP address and replies to wherever the phone's audio came from. The firewall must also allow the provider's RTP UDP port range. If SIP ALG on the new firewall is rewriting packets badly, turn it off.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c920ade0-50a2-4281-bdb3-79714daf7a0d"),
                column: "Explanation",
                value: "Rules run in priority order, starting from 0. Each rule has an option called 'Stop processing more rules'. When a message matches a rule with that option on, no lower rule is evaluated for that message. 'Vendor Bypass' matches only vendor.example, so if it sits above the disclaimer rule and has the option on, it explains why exactly one sender loses the disclaimer.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("caf329a9-cc48-c862-b83a-adbafdf11b1b"),
                column: "Explanation",
                value: "An NDR, or bounce message, carries an enhanced status code. 5.x.x is a permanent failure, and 5.7.x is a security or policy block. 5.7.520 comes with the text 'Access denied, your organization does not allow external forwarding'. So Exchange Online in your own tenant refused the message before it left, and nothing at the partner is at fault. Reading the code first tells you which side to investigate.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"),
                column: "Explanation",
                value: "RDP needs a listener on the server, which by default waits on TCP port 3389. qwinsta, short for query session, lists sessions and listeners. A line for rdp-tcp with the state Listen means the Remote Desktop service is up and accepting connections. If it's missing, the fault is on the server: RDP is disabled (fDenyTSConnections set to 1) or the TermService service is stopped. If it's there, the fault is between the client and the listener.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cb94ec6e-cd30-6075-cc2b-8567ecd3858b"),
                column: "Explanation",
                value: "There are two ways to send for another address. With Send As, the message looks as if it came from support@contoso.example and the delegate isn't shown anywhere. With Send on Behalf, the From line reads 'Aisha on behalf of Support'. The customer must not see Aisha, so it has to be Send As. Full Access is a third permission, for opening a mailbox, and it doesn't include sending.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"),
                column: "Explanation",
                value: "Event 4740 gave you the computer. Event 4625, 'An account failed to log on', in that computer's Security log gives you the kind of logon. Its Logon Type field tells you what is doing it: 2 is someone at the keyboard, 3 is a network connection such as a mapped drive, 4 is a scheduled task, 5 is a service, and 10 is Remote Desktop. The event also names the process.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cf9e64f8-d1aa-bcf6-5093-22a47eb5ba6f"),
                column: "Explanation",
                value: "Creating it under Teams & groups > Active teams & groups in the Microsoft 365 admin center provisions the mailbox, calendar, SharePoint site and Planner together. You choose a name, an email address, owners, members, and whether it's public or private. A group created with a team in Teams or a plan in Planner is the same kind of object underneath.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"),
                column: "Explanation",
                value: "The label isn't a fault to fix. Someone classified this team as internal only. If the business now needs a partner in it, the data owner has to accept that and approve a label whose settings allow guests. Then you change the team's label, and adding the guest works. Removing the label or creating an internal account gets the same result by going around the control, and an interviewer will be listening for that difference.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d243f093-453e-c659-371e-cbfea2afe06f"),
                column: "Explanation",
                value: "gpresult shows what Group Policy actually did on this PC for this user. The /h switch writes an HTML report that lists the applied GPOs, the denied GPOs with a reason such as security filtering or access denied, and the user's group memberships. Run it as the affected user. It replaces guessing in the management console with evidence from the client.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"),
                column: "Explanation",
                value: "WinSxS is the component store, which holds every version of Windows components so that updates can be installed and rolled back. It must only be cleaned with DISM. /AnalyzeComponentStore reports its real size and whether cleanup is recommended. /StartComponentCleanup then removes versions that newer updates have replaced. Deleting files in it by hand can leave Windows unable to update or boot.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d5b4ee7f-c21d-fb94-eb7d-08b3019e91d1"),
                column: "Explanation",
                value: "A rule's condition decides which messages it touches. 'The sender is located > Outside the organization' matches mail from any sender who isn't in one of the tenant's accepted domains, which means inbound external mail. Getting the direction right matters. The recipient version of this condition would match your own outbound mail.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"),
                column: "Explanation",
                value: "Open services.msc, go to the service's properties, and on the Log On tab enter the new password for CORP\\svc-sync. Then start the service. From the command line it's sc.exe config with obj= and password=. Don't keep retrying the start with the old password, because each attempt is a failed logon and can lock the account.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("dcffec3d-d15e-05e2-0909-5624f1040fe4"),
                column: "Explanation",
                value: "Match the group type to what the team needs. A distribution list is only an address that delivers mail to its members. A mail-enabled security group does that and can also be used to grant permissions. A Microsoft 365 Group creates a shared mailbox and calendar, a SharePoint site for files, and a Planner plan, all with one membership. The Events team asked for all four, so only the Microsoft 365 Group fits.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"),
                column: "Explanation",
                value: "When the phone registers, the home router creates a NAT mapping from the phone to the PBX, and the PBX sends calls back through that mapping. The router deletes an idle UDP mapping after 60 seconds. The registration stays valid for much longer, so the PBX keeps sending INVITEs to a mapping that no longer exists, and the router drops them. A keep-alive every 20 to 30 seconds keeps the mapping open. This explains the other clues too. A reboot re-registers and opens a new mapping for a minute, and outbound calls open their own.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"),
                column: "Explanation",
                value: "MTR combines traceroute and ping. It finds every router on the path and then probes each one many times, reporting loss and latency per hop. In this command, -r is report mode, -w keeps full hostnames, and -c 200 sends 200 probes to each hop. With 200 probes, a 5% loss shows up reliably.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"),
                column: "Explanation",
                value: "Deal with the cause, then the symptom, then verify. Update the task's stored password, or better, remove it or run it under a service account so it isn't tied to a person. Then unlock Dana's account. Then watch the PDC emulator for new 4740 events for a few cycles. If any appear with a different caller computer, there's a second source, such as a phone or a mapped drive.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e1eddee0-50d8-68e2-1a79-66a4dc743b1f"),
                column: "Explanation",
                value: "A new rule can run in three modes: Enforce, Test with Policy Tips, and Test without Policy Tips. In a test mode the rule's actions aren't applied, but matches are recorded, so message trace shows what it would have done. Once the matches look right, switch to Enforce and check that the rule is enabled. A wrong condition on an enforced rule changes every message at once.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"),
                column: "Explanation",
                value: "SIP response codes follow the same pattern as HTTP. 4xx is a request failure, and 408 Request Timeout means a request was sent and no final response came back in time. It doesn't say why. A 408 is generated by whichever device gave up waiting, so the work is to find how far the INVITE got.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e7de2726-9058-4dab-6026-21eb4d335180"),
                column: "Explanation",
                value: "Connect-ExchangeOnline comes from the ExchangeOnlineManagement module, which you install once with Install-Module ExchangeOnlineManagement. It signs in with modern authentication, so MFA works, and it loads the Exchange cmdlets such as Get-InboxRule and Set-Mailbox. The older ways in are gone. Basic authentication for remote PowerShell is switched off, and the MSOnline module is retired.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"),
                column: "Explanation",
                value: "Every domain controller forwards a failed password to the PDC emulator, because it always holds the newest password, and lockouts are recorded there. Find it with (Get-ADDomain).PDCEmulator. In its Security log, event 4740, 'A user account was locked out', has a Caller Computer Name field with the computer the bad attempts came from. The event is only written if Audit User Account Management is turned on.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"),
                column: "Explanation",
                value: "Test what was broken, in both directions. Make an outbound call and an inbound call through the PBX, and check that both people can hear each other. Then confirm in a capture that there are two RTP streams, one each way. Registration and internal calls worked during the fault, so they can't prove the fix.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ef96842e-8c24-101f-8659-f94681f081eb"),
                column: "Explanation",
                value: "Logon type 4 is a batch logon, which is what Task Scheduler uses to run a task as a stored account. The task saves the password when it's created and never learns about a change. Each run sends the old password, and a trigger every 20 minutes matches the lockout interval. Dana covered that PC last week, which is when she would have set it up.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"),
                column: "Explanation",
                value: "With -Repair, the cmdlet sets a new machine password and writes it to both the PC and its account in Active Directory, using the domain credential you supply. The computer object, its SID and its group memberships stay as they are, and normally no restart is needed. Reset-ComputerMachinePassword is the alternative. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f2457426-5608-421d-48c7-4da2540fc244"),
                column: "Explanation",
                value: "SharePoint sharing is decided in layers. The organization setting is the most permissive level any site can have. Each site has its own level, which can be the same or stricter. Domain allow or block lists can then restrict which external domains are permitted, at either level. One partner works and the other doesn't, so the block depends on the domain, and the site's settings and the domain limits are where to look.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"),
                column: "Explanation",
                value: "In PEAP and EAP-TLS, the RADIUS server presents its certificate first, and the client checks it before sending any credentials. The Mac's configuration profile lists exactly which CA and server names to trust. The renewed certificate comes from a chain that isn't in that list, so the Mac ends the handshake with an unknown CA alert. The password is never sent, so it can't be the problem.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f430db22-4555-1b5b-092d-f0e293522887"),
                column: "Explanation",
                value: "A sensitivity label applied to a team is a container label. It doesn't encrypt anything by being there. It sets rules for the team itself, including privacy and whether external users can be added. If the label's external user access option is off, owners can't add guests to that team, even though the tenant allows guests. You check this on the label in the Microsoft Purview portal, in its groups and sites settings.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f50ff392-2197-95b9-eaa7-2b62e0d10928"),
                column: "Explanation",
                value: "Policy changes in Microsoft 365 aren't instant, so wait before judging the result. Then test the real path. Send Marco a message that matches his rule, and run a message trace on it. The trace should show the original delivery and a second, forwarded message to the partner's address, with the status Delivered and no 5.7.520.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f76aa264-8000-fdc4-00be-401c07f07021"),
                column: "Explanation",
                value: "The action 'Apply a disclaimer to the message' has two forms. Append adds the text at the bottom, and prepend adds it at the top. Security asked for the bottom, so append. Some messages can't be changed, for example encrypted or signed ones, and the fallback action covers them. Wrap puts the original inside a new message that carries the disclaimer, Ignore delivers it with no disclaimer, and Reject bounces it. Wrap keeps the warning without losing mail.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"),
                column: "Explanation",
                value: "Windows has built-in firewall rules for Remote Desktop, in the Remote Desktop group, including Remote Desktop - User Mode (TCP-In) for port 3389. Enabling that group for the active profile, usually Domain, allows RDP and nothing else. In PowerShell: Get-NetFirewallRule -DisplayGroup \"Remote Desktop\" | Set-NetFirewallRule -Enabled True. The broader options work too, but they open far more than the one port.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"),
                column: "Explanation",
                value: "Error 1069 only says that the service's account couldn't log on. The Service Control Manager writes the reason to the System event log. Event 7038 names the account and gives the cause: wrong password, account disabled, account locked out, or password must be changed. Event 7041 means the account lacks the 'Log on as a service' right. Filter the System log by the source Service Control Manager.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("fce75af6-65e2-355a-ba86-18691e800a33"),
                column: "Explanation",
                value: "The Tenant Allow/Block List, under Threat policies in the Defender portal, takes block entries for domains and addresses, URLs, and files. A domain block stops further mail from payroll-portal.example. A URL block makes Safe Links stop the click, which also covers a copy sent from a different address. The sender passed SPF and DKIM for a domain the attacker owns, so authentication checks won't catch it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ff17c50d-64bc-3ab3-c65c-1ab209a66d20"),
                column: "Explanation",
                value: "Mail flow rules, also called transport rules, run on Exchange Online's servers while a message is in transit. That means they apply to every message for every user, whatever app or device is used, and users can't turn them off. Each rule has conditions, actions and optional exceptions. They are created in the Exchange admin center under Mail flow > Rules, or with New-TransportRule.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ff244dd0-d2b0-17b4-ffcd-87eee615d266"),
                column: "Explanation",
                value: "The service plan switches that exist for a single user also exist on a group's license assignment. Clearing Viva Engage there applies to every current and future member. If a user holds the same license from two places, the enabled plans are combined, so a plan only stays off if every assignment leaves it off.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("015dd9c5-6161-0c58-b668-a5b11b501069"),
                column: "Explanation",
                value: "Disabling individual service plans within the license is the supported way to give a user a subset of a subscription.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"),
                column: "Explanation",
                value: "A click through to a credential page may have captured a password, and revoking sessions cuts off any tokens the attacker already has.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03fe44d1-045b-dd43-5c2a-8f28a9bf0e88"),
                column: "Explanation",
                value: "Delegate permissions for a group, including Send As, are managed on the group in the Exchange admin center.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"),
                column: "Explanation",
                value: "The DNS server gave the right answer directly. Only the local path that apps use is still wrong, so the fault is on the Mac.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("063e942b-84c2-66c3-3815-01f571f03414"),
                column: "Explanation",
                value: "The exception covers only the approved user for the approved period, and the policy keeps protecting everyone else.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0746394d-1ec2-c78f-4b8e-ed734c66af0f"),
                column: "Explanation",
                value: "Membership drives the assignment, so joiners get the license and leavers release it without any admin effort.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"),
                column: "Explanation",
                value: "ResetBase frees the most space, but it removes the rollback path for existing updates. Updates installed later can still be uninstalled.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("08877fab-aa11-4280-5064-4ea65ce68431"),
                column: "Explanation",
                value: "The service is healthy and the port is unreachable from the same subnet, which leaves little other than the host firewall.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"),
                column: "Explanation",
                value: "Measuring first shows where the space went, so that you remove the right thing and can explain the cause.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"),
                column: "Explanation",
                value: "Country locations come from the sign-in's public IP. Luis's IP maps to Portugal, which the policy blocks.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"),
                column: "Explanation",
                value: "The cmdlet tests the channel between the local computer and its domain, and returns False when it's broken.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1321e4a7-c19a-7752-68a2-8d1f4257a090"),
                column: "Explanation",
                value: "A custom policy allows the exception for one approved user and leaves the secure default in place for everyone else.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("134765e8-456a-7523-7bb0-d74aabba779b"),
                column: "Explanation",
                value: "Entra ID records assignment errors per user in the group's licensing view, along with the reason for each.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"),
                column: "Explanation",
                value: "This flushes the directory service cache, then signals mDNSResponder to drop its cached DNS results.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1823c892-3611-380b-80d0-0441fa9616af"),
                column: "Explanation",
                value: "Read without Apply group policy lets the PCs fetch the GPO, while Security Filtering still limits who it applies to.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"),
                column: "Explanation",
                value: "A connected session isn't re-validated continuously. The new certificate only comes into play at the next authentication, which fits the 8-hour pattern.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"),
                column: "Explanation",
                value: "The token is a snapshot of group membership at sign-in. The server checks his token, and it doesn't contain the group yet.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("22105984-345f-f942-460c-ef2a7966ae39"),
                column: "Explanation",
                value: "This gives the ISP proof that the loss starts at its handoff and that your side has been ruled out, which shortens the investigation.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"),
                column: "Explanation",
                value: "Old web logs are the largest item and are safe to remove once retention needs are met. This frees space with no risk to Windows.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"),
                column: "Explanation",
                value: "Retransmission means nothing came back. A phone that received the INVITE would answer with a provisional response, and 32 seconds matches Timer B.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2c7d8b12-d8a4-1d97-e00a-9bf9d70aa62c"),
                column: "Explanation",
                value: "What If simulates the sign-in against every policy, so it proves the exclusion works. Luis's next sign-in log entry then confirms it in production.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"),
                column: "Explanation",
                value: "A clean summary with a fresh success time shows the backlog has been delivered, including this morning's accounts.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("381ec257-5b7d-2896-21d5-ba29c9e460f2"),
                column: "Explanation",
                value: "Permissions can take a while to take effect, and Outlook sends from the user's own address unless the From field is changed.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("399d9d86-b7dd-b66a-6074-15cddd0bf5bc"),
                column: "Explanation",
                value: "Automatic forwarding rules are configured in the outbound spam filter policy, under Threat policies > Anti-spam.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"),
                column: "Explanation",
                value: "The same tests that failed should now pass. Fixing the script or GPO behind the change stops it from disabling the rule again.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"),
                column: "Explanation",
                value: "The sign-in log entry shows the error code, the location, and the result of every policy evaluated, including the one that blocked access.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3d623bd9-091b-bf4a-8d79-7c3b087fb6ee"),
                column: "Explanation",
                value: "Either change lets the disclaimer rule run while keeping the vendor rule's other behavior.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"),
                column: "Explanation",
                value: "These are the two sets that decide access. Effective Access calculates the NTFS result for his account.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"),
                column: "Explanation",
                value: "The fault appeared at reauthentication, so a successful grant after a full 8-hour session is the evidence that counts.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("464963e8-41e7-2f0a-b974-f85e9779d400"),
                column: "Explanation",
                value: "This admits exactly the approved partner, and keeping the Entra list in step prevents a second block when the guest is invited.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("46baf750-afaa-a6df-25f8-be25c4030396"),
                column: "Explanation",
                value: "1722 means the RPC server is unavailable. It's a connection failure, not a problem with the directory data.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"),
                column: "Explanation",
                value: "The link and the group membership are right, so permissions on the GPO are what's left, and access denied points the same way.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"),
                column: "Explanation",
                value: "Existing clients working suggests the server is reachable. Scope statistics show right away whether the pool has run out of addresses.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("497b1140-0863-3bb1-9706-231416a7b32c"),
                column: "Explanation",
                value: "Removing the rule and forwarding stops the hiding, while the password reset and session revocation lock out the attacker so the rule can't be recreated.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"),
                column: "Explanation",
                value: "Authentication has two sides. The Mac shows what the client saw, and RADIUS shows why the request was accepted or rejected.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"),
                column: "Explanation",
                value: "If hop 5 were really dropping traffic, later hops would show loss too. It's just giving low priority to replies aimed at itself.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4fbf285b-f7f3-0850-2261-611d7f98d9cc"),
                column: "Explanation",
                value: "Submitting teaches the filters, and the allow entry is targeted and expires, so it is the Microsoft-recommended way to handle a false positive.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"),
                column: "Explanation",
                value: "A SIP ladder shows each request and response in order, so you can see exactly what the PBX sent and what it got back.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"),
                column: "Explanation",
                value: "DNS lookup failures are behind a large share of 1722 errors, so name resolution is the first thing to rule in or out.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"),
                columns: new[] { "CorrectAction", "Explanation", "Prompt" },
                values: new object[] { "Both are checked, and the more restrictive of the two results applies", "A user has to pass both. Whichever set allows less decides what they can do.", "How do share permissions and NTFS permissions combine when someone connects over the network?" });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"),
                column: "Explanation",
                value: "It summarizes failures and the largest replication delay for every domain controller, so you can see at once which ones are behind.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5fe20ef1-1f7b-cd82-6161-354cf1b74257"),
                column: "Explanation",
                value: "Trace details record the mail flow rules that were applied to the message, so you can see what ran.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("601ffe8b-0de5-9e57-9875-6f9be645da90"),
                column: "Explanation",
                value: "Filtering on both parties and the right window returns only the relevant messages, and the sender and recipient appear exactly as Exchange Online recorded them.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("60dbfd7e-99e4-5f80-f1f6-e7abed929318"),
                column: "Explanation",
                value: "Guest membership lives in the host tenant, and Teams only shows that tenant's teams after the guest switches to it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"),
                column: "Explanation",
                value: "A one-character name, a hidden rule, and finance keywords moved to an unused folder are classic signs of business email compromise.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"),
                column: "Explanation",
                value: "Only the internal DNS servers hold the domain's records. Clearing the cache with ipconfig /flushdns removes the failed lookups.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d2abeae-2397-c7a3-b761-089f45177081"),
                column: "Explanation",
                value: "Inbox rules act after delivery, and -IncludeHidden returns rules that Outlook doesn't display. The Description property spells out each rule's conditions and actions.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"),
                column: "Explanation",
                value: "The SDP c= line tells the other side where to send audio. A 192.168.x.x address means nothing on the internet, so the return stream never arrives.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"),
                column: "Explanation",
                value: "Visitors' devices keep their addresses long after they've left, so the pool fills up with leases nobody is really using.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"),
                column: "Explanation",
                value: "A call that rings and connects has completed its SIP exchange. Audio travels in a separate RTP stream that can fail on its own.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"),
                column: "Explanation",
                value: "Explorer returns every matching message with its recipients, delivery location and URL clicks in one view.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"),
                column: "Explanation",
                value: "Loss that starts at a hop and continues to the destination is genuine. Here that places it at the firewall-to-ISP link or the ISP's gateway.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72bf031d-2d13-1a34-549a-b71b4faffb68"),
                column: "Explanation",
                value: "Quarantined messages are held in the Defender portal. Reviewing them first confirms that they really are from the supplier before they reach the user.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72ec1818-d0bc-e20e-0322-953997a0d723"),
                column: "Explanation",
                value: "Remediating from Explorer pulls delivered copies out of every mailbox at once, and the Action center records the result.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("7baf03c7-e445-9275-d136-72e655c7b8f2"),
                column: "Explanation",
                value: "The problem appears to be on the service side, and message trace is the record of how Exchange Online handled the message.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("801fb6ed-6c3f-95a7-9828-690e0b11d9ae"),
                column: "Explanation",
                value: "A real message and its trace prove the expected rule fired. Rule changes can take a short time to apply.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"),
                column: "Explanation",
                value: "Since MS16-072, the computer account needs Read on user GPOs. It had that through Authenticated Users until the group was removed.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("81b4863b-3305-e700-519f-7b935733ab79"),
                column: "Explanation",
                value: "The PC changed its password after the checkpoint was taken. Reverting put back the old one, while Active Directory kept the new one.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("827bc225-d24a-2406-a175-96abdf975124"),
                column: "Explanation",
                value: "dig asks the DNS server directly, while dscacheutil goes through the macOS resolver that apps use. A difference between them points at the Mac.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("838d0d9a-847e-2dbd-3839-3b5ba965d837"),
                column: "Explanation",
                value: "Once the missing property is set, the group can assign the license. Reprocessing gets it done without waiting.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7412cf-1371-43c7-2d96-9fc2d62e9afb"),
                column: "Explanation",
                value: "Only the full path proves it works, from invitation to sign-in to file access. Sharing changes can take a few minutes to apply.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"),
                column: "Explanation",
                value: "The system resolver consults more than the DNS cache. A hosts entry overrides DNS, and a VPN can add a scoped resolver for a domain.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8abc1259-4578-20fe-b5b3-92e0d403c3cb"),
                column: "Explanation",
                value: "An allow list blocks every domain that isn't on it, which is why adatum.example works and fabrikam.example doesn't.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"),
                column: "Explanation",
                value: "The share already allows more than that, so NTFS decides the result. A group keeps the permission manageable as auditors come and go.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("941e7c17-066e-1729-ea54-2d58af65c26d"),
                column: "Explanation",
                value: "This wizard handles the account, the license assignment, and the per-app settings in one flow.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("94417086-b2e1-926e-f906-10354c29eed4"),
                column: "Explanation",
                value: "The logs caused the incident, so they need a limit. Earlier alerts leave time to act before anything fails.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("981d38df-be64-d403-71f2-931cdbd8145e"),
                column: "Explanation",
                value: "Signing in again builds a token that includes Finance-RW, and the existing permissions then let him in.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"),
                column: "Explanation",
                value: "The service signed in before the rotation and kept running. The restart was the first time Windows tried the saved password again.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"),
                column: "Explanation",
                value: "A capture shows the SDP each side advertised and which RTP streams actually exist. Wireshark's Telephony > VoIP Calls view lays this out for each call.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"),
                column: "Explanation",
                value: "The computer proves its identity to the domain with its own account password. When the two sides disagree, the secure channel fails and so do domain sign-ins.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"),
                column: "Explanation",
                value: "A local account is checked against the PC's own account database, so it doesn't need the secure channel.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"),
                column: "Explanation",
                value: "The fault only appears once the mapping has aged out, so the test has to wait for that. A ladder with provisional responses confirms the path stayed open.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"),
                column: "Explanation",
                value: "A shorter lease stops this from recurring, and removing stale leases frees addresses now. Conflict detection guards against an address still being in use.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"),
                column: "Explanation",
                value: "This quickly separates a LAN problem from a WAN problem. A larger count catches intermittent loss that a few packets would miss.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"),
                column: "Explanation",
                value: "Checking on a real sales PC as a sales user tests the exact path that was failing.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a36f3e7c-c851-b3ce-5b5f-8261ccd91317"),
                column: "Explanation",
                value: "Licenses can't be assigned without a usage location because service availability varies by country or region.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"),
                column: "Explanation",
                value: "The event names the configured password as the failure, and the account's password was rotated last week.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a7ba522e-0217-6b71-33a8-76cb23de155d"),
                column: "Explanation",
                value: "By default, a Microsoft 365 Group only accepts mail from internal senders. This setting opens it to external senders.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a80dc289-87e8-7b23-fa54-93d9b8b1f698"),
                column: "Explanation",
                value: "In Exchange Online, Add-RecipientPermission is the cmdlet that grants Send As.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"),
                column: "Explanation",
                value: "A lockout on a timer has a machine behind it. Until that source is found, each unlock only lasts until its next attempt.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"),
                column: "Explanation",
                value: "Both add real capacity. A /23 means changing the gateway mask and the scope together, while a guest VLAN also keeps visitors away from staff devices.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"),
                column: "Explanation",
                value: "Interface errors, a duplex mismatch or a full link would mean the fault is yours. Return paths can differ, so a reverse MTR gives the ISP the full picture.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b70a712e-865b-04bf-a392-cb31612c6602"),
                column: "Explanation",
                value: "With a gMSA, Active Directory rotates the password and the server retrieves it, so there's no saved password to go stale.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b731b4cb-9f14-159f-5c15-56786d65694d"),
                column: "Explanation",
                value: "Owners keep the group's membership and settings current without admin involvement, and a second owner guards against orphaning it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"),
                column: "Explanation",
                value: "Clients use APIPA when DHCP fails. It's a useful clue, since it means the problem is getting a lease, not routing or DNS.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"),
                column: "Explanation",
                value: "A local connection proves the RDP service works. Test-NetConnection with -Port 3389 from elsewhere then tests the path to it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c4e4078b-5ba5-d86b-f7e4-2d13793e498a"),
                column: "Explanation",
                value: "This view shows what's actually provisioned for the user, including each service plan's state.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c592c3bc-9a51-6552-3f57-d4dcb0433bcd"),
                column: "Explanation",
                value: "Guest blocks come either from tenant-wide settings or from one team's label or group setting. One test tells you which layer to inspect.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"),
                column: "Explanation",
                value: "This restores proper validation on every managed Mac at once, and users don't need to make any trust decisions.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"),
                column: "Explanation",
                value: "STUN, or an SBC or PBX using symmetric RTP, gets audio back through NAT. The firewall must also pass the provider's RTP UDP range.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c920ade0-50a2-4281-bdb3-79714daf7a0d"),
                column: "Explanation",
                value: "A higher-priority rule with this option on prevents later rules from running for messages it matches.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("caf329a9-cc48-c862-b83a-adbafdf11b1b"),
                column: "Explanation",
                value: "The text of this NDR is 'Access denied, your organization does not allow external forwarding', so the block is in your tenant rather than at the partner.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"),
                column: "Explanation",
                value: "This shows whether the server is accepting RDP connections at all, which splits the problem into listener and network.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cb94ec6e-cd30-6075-cc2b-8567ecd3858b"),
                column: "Explanation",
                value: "Only Send As hides the delegate completely. The recipient sees just the group's address.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"),
                column: "Explanation",
                value: "Event 4625 on the source computer records each failed logon and its logon type, which shows what kind of thing is sending the password.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cf9e64f8-d1aa-bcf6-5093-22a47eb5ba6f"),
                column: "Explanation",
                value: "This is the standard admin path, and it provisions the connected group resources.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d084c57d-b6c7-1f50-07a0-f865cad1e9d1"),
                column: "Explanation",
                value: "The label records a classification decision, so the owner of the data must approve the change instead of IT working around it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d243f093-453e-c659-371e-cbfea2afe06f"),
                column: "Explanation",
                value: "The report shows what the client actually processed and the reason for anything it skipped, so you aren't guessing from the server side.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"),
                column: "Explanation",
                value: "The analysis reports what can be reclaimed, and the cleanup removes superseded component versions in the supported way.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d5b4ee7f-c21d-fb94-eb7d-08b3019e91d1"),
                column: "Explanation",
                value: "This matches mail whose sender is external to the tenant, which is exactly what Security requested.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"),
                column: "Explanation",
                value: "This brings the saved password back in line with the domain, which is the mismatch the event described.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("dcffec3d-d15e-05e2-0909-5624f1040fe4"),
                column: "Explanation",
                value: "It's the only option here that includes a shared mailbox, calendar, files, and a Planner board.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"),
                column: "Explanation",
                value: "CRLF or OPTIONS keep-alives refresh the NAT mapping, so inbound INVITEs have an open path. This also explains why rebooting helps for a short time.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"),
                column: "Explanation",
                value: "Report mode with 200 probes per hop gives a statistically useful picture of loss at every hop on the path.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"),
                column: "Explanation",
                value: "Removing the source stops the bad attempts. Watching for further lockout events proves that there wasn't a second source.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e1eddee0-50d8-68e2-1a79-66a4dc743b1f"),
                column: "Explanation",
                value: "Test mode shows how the rule matches without changing live mail. New rules can also start disabled, so check that it's enabled.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"),
                column: "Explanation",
                value: "408 is about time running out. The request got no final answer, so the next job is to find which hop it stopped at.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e7de2726-9058-4dab-6026-21eb4d335180"),
                column: "Explanation",
                value: "The Exchange Online PowerShell V3 module signs in with modern authentication, supports MFA, and loads the Exchange Online cmdlets.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"),
                column: "Explanation",
                value: "Lockouts are processed on the PDC emulator, and event 4740 there records the computer that the failed attempts came from.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"),
                column: "Explanation",
                value: "Testing inbound and outbound calls covers both call paths, and the capture proves media flows each way instead of relying on one good impression.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ef96842e-8c24-101f-8659-f94681f081eb"),
                column: "Explanation",
                value: "Logon type 4 is a batch logon, which is how scheduled tasks sign in. The regular interval matches a task trigger.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"),
                column: "Explanation",
                value: "This resets the machine account password on both sides in place, usually with no restart and with the computer object left intact.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f2457426-5608-421d-48c7-4da2540fc244"),
                column: "Explanation",
                value: "Below the organization level, the site setting and domain restrictions are the layers that can block one partner but not another.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"),
                column: "Explanation",
                value: "An unknown CA alert means the client rejected the server's certificate. The timing matches the renewal, and the Macs are the ones using a fixed trust profile.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f430db22-4555-1b5b-092d-f0e293522887"),
                column: "Explanation",
                value: "Container sensitivity labels can control guest access for a Microsoft 365 group, and they override what the owner can do in that team.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f50ff392-2197-95b9-eaa7-2b62e0d10928"),
                column: "Explanation",
                value: "Policy changes can take a little while to apply. A trace proves that the forwarded copy left the tenant and was delivered.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f76aa264-8000-fdc4-00be-401c07f07021"),
                column: "Explanation",
                value: "Append places the text at the end of the message, and Wrap makes sure that a message that can't be modified still carries the warning in an outer message.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"),
                column: "Explanation",
                value: "The built-in rules allow exactly the traffic RDP needs. Get-NetFirewallRule -DisplayGroup \"Remote Desktop\" | Set-NetFirewallRule -Enabled True does it in one line.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"),
                column: "Explanation",
                value: "Error 1069 is generic. The Service Control Manager event states which account failed to log on and why.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("fce75af6-65e2-355a-ba86-18691e800a33"),
                column: "Explanation",
                value: "Block entries stop new mail from the domain, and Safe Links blocks the URL at click time even in messages that get through.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ff17c50d-64bc-3ab3-c65c-1ab209a66d20"),
                column: "Explanation",
                value: "Mail flow rules live in the Exchange admin center and apply at the transport level, so they cover every client.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ff244dd0-d2b0-17b4-ffcd-87eee615d266"),
                column: "Explanation",
                value: "The group's assignment options apply the same service plan configuration to every member.");
        }
    }
}
