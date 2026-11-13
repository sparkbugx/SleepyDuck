using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveDirectoryAndWindowsServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScenarioCards",
                columns: new[] { "Id", "Category", "ProblemStatement", "TechnicalExplanation", "Title" },
                values: new object[,]
                {
                    { new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3"), "WindowsServer", "Monitoring reports that the C: drive on WEB-SRV01 has 1% free space. Windows Update is failing on the server, and the web application has started logging write errors.", "A full system drive stops updates, logging, and sometimes the applications themselves, so act quickly, but measure before deleting anything. List the largest folders first. On servers the space usually goes to logs that nobody rotates, such as IIS logs under C:\\inetpub\\logs\\LogFiles, along with dump files, temp folders and old installers. The WinSxS folder is the Windows component store. It holds the files Windows needs to service itself, and Microsoft warns that deleting files from it can leave a system unable to boot or update. Reduce it with the built-in tools. Dism /Online /Cleanup-Image /AnalyzeComponentStore reports how much can be reclaimed, and /StartComponentCleanup removes superseded component versions straight away, where the scheduled task waits 30 days. Adding /ResetBase removes every superseded version, and after that the updates already installed can't be uninstalled. Then fix the cause: put retention on the logs or move them to a data volume, and alert on free space well before it's critical.", "System drive full on a web server" },
                    { new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e"), "WindowsServer", "After Sunday's patching restart, the Contoso Sync service on APP-SRV02 won't start. Starting it by hand returns 'Error 1069: The service did not start due to a logon failure'. The service runs as the domain account CORP\\svc-sync, and the security team rotated service account passwords last week.", "A Windows service that runs as a user account stores that account's password in its own configuration, and Windows uses it to sign the account in each time the service starts. Error 1069 is the general message. The Service Control Manager writes the specific reason to the System event log. Event 7038 says the service was unable to log on with the currently configured password, followed by the cause: the user name or password is incorrect, the account is disabled, the account is locked out, the password must be changed, or the domain could not be contacted. Event 7041 is a different problem, where the account lacks the Log on as a service right. Changing a password in Active Directory doesn't update services that use it. A service that is already running keeps its session, so the mismatch only shows at the next start, often after a patching restart days later. Fix it by entering the new password on the service's Log On tab. Repeated start attempts with the old password can lock the account. A group managed service account avoids all this, because Active Directory rotates its password and the host retrieves it automatically.", "Service fails to start with a logon failure" },
                    { new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e"), "WindowsServer", "Tomas joined Accounting today and gets 'Access is denied' when he opens \\\\FILE-SRV01\\Finance. His colleagues can open it. He was added to the Finance-RW group an hour ago and has been signed in since this morning.", "A shared folder has two sets of permissions. Share permissions apply only to access over the network, and NTFS permissions apply to every kind of access. Over the network both are evaluated and the more restrictive result wins, so Full Control on the share with Read in NTFS gives Read. Common practice is to keep share permissions broad, for example Authenticated Users with Full Control or Change, and to do the real control in NTFS with groups. Within one set, a user's permissions from all their groups add up, but a Deny entry beats any Allow. The Effective Access tab under Advanced Security Settings calculates the NTFS result for a named user. Group membership has its own catch. A user's access token, which lists their groups, is built when they sign in. Adding someone to a group changes Active Directory, but not a token that already exists, so the user has to sign out and back in. whoami /groups shows the groups in the current token. Grant access to groups and not to individual accounts, or the permissions become unmanageable.", "Access denied on a file share" },
                    { new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb"), "ActiveDirectory", "Accounts created this morning on HQ-DC01 can't sign in at the branch office, where PCs use the local domain controller BR-DC02. Password resets made at headquarters aren't working at the branch either. Internet access at the branch is fine.", "Domain controllers copy changes to each other, and when that stops, each site slowly works from a different version of the directory. Start with repadmin /replsummary for a forest-wide view of failures and how old they are, then repadmin /showrepl on the failing DC for each partition's last attempt, result, and last success. Error 1722, 'The RPC server is unavailable', is a connectivity error: the destination DC couldn't connect to the source DC. Microsoft notes that DNS lookup failures cause a large share of these, because DCs locate each other through records in the _msdcs zone. dcdiag /test:dns checks that, and nltest /dsgetdc tests the DC locator. A DC must use internal DNS servers that host the Active Directory zones. A public resolver doesn't hold those records, and an alternate server is only tried when the preferred one doesn't answer. Other causes of 1722 are blocked ports, namely TCP 135 for the endpoint mapper and the dynamic RPC range. Don't leave replication broken for long. A DC that stays out of sync past the tombstone lifetime, usually 180 days, can bring deleted objects back.", "New accounts missing at a branch (replication error 1722)" },
                    { new Guid("652a4793-984d-cf69-18d2-336dda12226c"), "ActiveDirectory", "Dana changed her domain password on Monday. Since then her account locks out every 20 minutes or so, even while she's away from her desk. The help desk has unlocked it six times, and each unlock lasts only a few minutes.", "An account locks when the count of bad passwords passes the lockout threshold, so a lockout that repeats on a timer means something is still sending the old password. A domain controller that sees a bad password checks it with the PDC emulator, which always has the newest password, and lockouts are processed there. That makes the PDC emulator's Security log the place to look for event 4740, 'A user account was locked out'. Find that DC with (Get-ADDomain).PDCEmulator. The event's Caller Computer Name field names the computer the failed attempt came from. On that computer, event 4625 records each failed logon with its logon type: 2 is interactive, 3 is network, 4 is batch (scheduled tasks), 5 is service, and 10 is Remote Desktop. Usual culprits are scheduled tasks, services, mapped drives, saved entries in Credential Manager, disconnected Remote Desktop sessions, and mail apps on phones. Event 4740 is only logged when Audit User Account Management is enabled.", "Account keeps locking after a password change" },
                    { new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f"), "ActiveDirectory", "A colleague built a GPO called Sales Drive Map with user settings and linked it to the Staff OU. To limit it to the sales team, they removed Authenticated Users from Security Filtering and added the Sales-Users group. Nobody in Sales gets the drive, and gpupdate /force doesn't help.", "A GPO applies to a user only if it is linked where the user's account lives, and the user has both Read and Apply group policy permissions on it. Security Filtering is a shortcut for granting those two permissions. There's one more requirement that catches people out. Since security update MS16-072 in June 2016, Windows fetches user policy with the computer's security context, where it used to use the user's. The computer account therefore needs Read permission on any GPO that applies to the signed-in user. By default it gets that through Authenticated Users, a group that includes computer accounts. Removing Authenticated Users from Security Filtering takes away Read as well as Apply. The fix is on the GPO's Delegation tab: add Authenticated Users or Domain Computers with Read only. That lets computers read the GPO without applying it to everyone. On the client, gpresult /h produces a report of which GPOs were applied or denied, and why. A Deny entry always beats an Allow.", "Group Policy isn't applying to a security group" },
                    { new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24"), "WindowsServer", "Since last night's security hardening change, no administrator can open a Remote Desktop session to FILE-SRV03. The client reports 'Remote Desktop can't connect to the remote computer'. The server answers ping, and its file shares still work. You have access to its Hyper-V console.", "This error means the client never reached a working RDP listener, and Microsoft's guidance splits the causes into the listener and the network. With console access, run qwinsta on the server. A line for rdp-tcp in the Listen state means the listener is up. If it's missing, check that fDenyTSConnections is 0, both under HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server and in the policy path if one is set, and that the Remote Desktop Services service (TermService) is running. Next, connect from the console with mstsc to localhost. If that works, the server side is healthy and the fault is between client and server. Test-NetConnection with -Port 3389 from another machine tests the TCP port itself, which ping can't do because ping uses ICMP. A False result points to the Windows Firewall rules named Remote Desktop - User Mode (TCP-In and UDP-In), a network firewall, or a changed port number. Failures after that point, such as credentials, Network Level Authentication, certificates, or membership of Remote Desktop Users, produce different messages because the connection has already been made.", "Remote Desktop can't connect to a server" },
                    { new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417"), "ActiveDirectory", "After a bad software install, a technician reverted the virtual desktop VDI-ACCT07 to a checkpoint taken ten weeks ago. Now every domain user who tries to sign in to it gets 'The trust relationship between this workstation and the primary domain failed'.", "Every domain-joined computer has its own account in Active Directory, with a password that the computer and the domain both know. The computer uses it to build a secure channel to a domain controller, and domain sign-ins on that computer depend on the channel. By default the computer itself starts a password change every 30 days. Active Directory never expires a machine password, so a PC that was simply switched off for months still works. The usual real causes are restoring an old checkpoint or image, which brings back a password the domain has since replaced, or a computer account that was reset, deleted, or taken over by another machine with the same name. Test-ComputerSecureChannel reports whether the channel is healthy, and adding -Repair with a domain credential resets it in place. Reset-ComputerMachinePassword is the next option. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort. Don't delete the computer object, since a new one has a different SID and loses its group memberships.", "Trust relationship with the domain failed" }
                });

            migrationBuilder.InsertData(
                table: "SequentialSteps",
                columns: new[] { "Id", "CorrectAction", "CorrectOptionId", "Explanation", "Order", "Prompt", "ScenarioCardId" },
                values: new object[,]
                {
                    { new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"), "It removes every superseded component version, so installed updates can't be uninstalled", new Guid("71b364e1-4744-e17d-408d-f410cf4533e9"), "ResetBase frees the most space, but it removes the rollback path for existing updates. Updates installed later can still be uninstalled.", 4, "A colleague suggests adding /ResetBase to the cleanup. What is the trade-off?", new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3") },
                    { new Guid("08877fab-aa11-4280-5064-4ea65ce68431"), "Something is blocking TCP 3389 before the listener, most likely the server's own firewall", new Guid("6191c191-c93c-870a-267b-8a6c3b4c3baa"), "The service is healthy and the port is unreachable from the same subnet, which leaves little other than the host firewall.", 3, "The localhost session works. From a PC on the same subnet, the port test returns TcpTestSucceeded: False. What does this show?", new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24") },
                    { new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"), "Find what is using the space by listing the largest folders on C:", new Guid("bebd0375-d682-c89d-bc9f-26128dd08d8b"), "Measuring first shows where the space went, so that you remove the right thing and can explain the cause.", 1, "What do you do first?", new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3") },
                    { new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"), "Run Test-ComputerSecureChannel -Verbose in an elevated PowerShell window", new Guid("984d5d00-801f-4c63-02f0-c1b1cf070eb4"), "The cmdlet tests the channel between the local computer and its domain, and returns False when it's broken.", 3, "How do you confirm that the secure channel is the problem?", new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417") },
                    { new Guid("1823c892-3611-380b-80d0-0441fa9616af"), "On the Delegation tab, add Domain Computers or Authenticated Users with Read only", new Guid("a326a61c-d914-7cea-726f-f8927dbd3049"), "Read without Apply group policy lets the PCs fetch the GPO, while Security Filtering still limits who it applies to.", 4, "What is the right fix?", new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f") },
                    { new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"), "His access token was built when he signed in, before he was added to the group", new Guid("4ed09213-02ba-7f1c-9b98-2b748666c403"), "The token is a snapshot of group membership at sign-in. The server checks his token, and it doesn't contain the group yet.", 3, "The share grants Authenticated Users Full Control, and NTFS grants Finance-RW Modify. On Tomas's PC, whoami /groups doesn't list Finance-RW. What is the cause?", new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e") },
                    { new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"), "Archive or delete the old IIS logs, keeping whatever the app owner must retain", new Guid("971f85df-0978-823c-6e1b-5b8fdf93a08f"), "Old web logs are the largest item and are safe to remove once retention needs are met. This frees space with no risk to Windows.", 2, "You find 38 GB of IIS logs going back four years in C:\\inetpub\\logs\\LogFiles, and a 14 GB WinSxS folder. What is the best first move?", new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3") },
                    { new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"), "Force a sync, then check repadmin /replsummary for zero failures and a current last success", new Guid("3ea280ee-ec6e-0471-0c9e-5d75fd7ad19e"), "A clean summary with a fresh success time shows the backlog has been delivered, including this morning's accounts.", 5, "How do you confirm that replication has recovered?", new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb") },
                    { new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"), "Repeat the port test, connect by RDP, and correct the hardening change so it can't recur", new Guid("7f28d5cb-4999-f0c9-3e5a-f0cf87eb2d02"), "The same tests that failed should now pass. Fixing the script or GPO behind the change stops it from disabling the rule again.", 5, "How do you finish the job?", new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24") },
                    { new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"), "On the server, review the share permissions and the folder's Security tab, with Effective Access", new Guid("b81509b3-a6c0-0081-5662-b40a9258417b"), "These are the two sets that decide access. Effective Access calculates the NTFS result for his account.", 2, "Where do you look to see what access Tomas should have?", new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e") },
                    { new Guid("46baf750-afaa-a6df-25f8-be25c4030396"), "BR-DC02 can't connect to the RPC service on its source DC, so suspect DNS or the network path", new Guid("a4693142-8050-b250-0992-fd85e70e386c"), "1722 means the RPC server is unavailable. It's a connection failure, not a problem with the directory data.", 2, "The summary shows BR-DC02 failing inbound replication for two days with error 1722. What does that error tell you?", new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb") },
                    { new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"), "The GPO's Delegation tab, to see whether computer accounts can read it", new Guid("40b679b9-2718-22b0-feec-7f34ec8f4771"), "The link and the group membership are right, so permissions on the GPO are what's left, and access denied points the same way.", 2, "The report doesn't list Sales Drive Map as applied, and it flags an access denied error for user policy. The user is in Sales-Users and in the Staff OU. What do you check?", new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f") },
                    { new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"), "Run dcdiag /test:dns /v on BR-DC02 and check it can resolve HQ-DC01's records", new Guid("aad7f01d-c2df-024f-23c2-123738c61365"), "DNS lookup failures are behind a large share of 1722 errors, so name resolution is the first thing to rule in or out.", 3, "What do you test first?", new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb") },
                    { new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"), "Both are checked, and the more restrictive of the two results applies", new Guid("0859d2cd-4953-bf70-a7ac-da7a15794396"), "A user has to pass both. Whichever set allows less decides what they can do.", 1, "How do share permissions and NTFS permissions combine when someone connects over the network?", new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e") },
                    { new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"), "repadmin /replsummary", new Guid("81f0cdf0-8994-9673-f5ec-3bd445189727"), "It summarizes failures and the largest replication delay for every domain controller, so you can see at once which ones are behind.", 1, "Which command gives you the quickest overall view of replication health?", new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb") },
                    { new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"), "Point BR-DC02 back at the internal AD DNS servers, then clear its resolver cache", new Guid("83170555-f267-28bf-4b29-837d61b1b5e9"), "Only the internal DNS servers hold the domain's records. Clearing the cache with ipconfig /flushdns removes the failed lookups.", 4, "The DNS test fails. BR-DC02's network adapter lists 8.8.8.8 as its preferred DNS server, set during an ISP outage last week. What is the fix?", new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb") },
                    { new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"), "User policy is now fetched as the computer, and no entry lets the PCs read this GPO", new Guid("cd5e3ac0-3706-65ed-9529-053e402b48c7"), "Since MS16-072, the computer account needs Read on user GPOs. It had that through Authenticated Users until the group was removed.", 3, "The Delegation tab lists Sales-Users, Domain Admins, Enterprise Admins, SYSTEM and Enterprise Domain Controllers. Why does that break the GPO?", new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f") },
                    { new Guid("81b4863b-3305-e700-519f-7b935733ab79"), "The checkpoint restored an old machine password that the domain had since replaced", new Guid("536fd4b0-9ef5-8ce3-43b4-06d1d634c344"), "The PC changed its password after the checkpoint was taken. Reverting put back the old one, while Active Directory kept the new one.", 5, "The manager asks why reverting the checkpoint caused this. What do you say?", new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417") },
                    { new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"), "Give an Auditors group Read & execute in the folder's NTFS permissions", new Guid("2c9c30d2-9f27-becc-0692-09dfdae14a80"), "The share already allows more than that, so NTFS decides the result. A group keeps the permission manageable as auditors come and go.", 5, "Later, auditors need read-only access to the same folder. How do you set it up?", new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e") },
                    { new Guid("94417086-b2e1-926e-f906-10354c29eed4"), "Set log retention or move logs off C:, and alert on free space much earlier", new Guid("16019707-f235-5b1e-9fa9-e98e2b644b9e"), "The logs caused the incident, so they need a limit. Earlier alerts leave time to act before anything fails.", 5, "Space is back to 45% free. How do you stop this from happening again?", new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3") },
                    { new Guid("981d38df-be64-d403-71f2-931cdbd8145e"), "Have Tomas sign out and sign back in, so that he gets a new token", new Guid("d1a8d064-0831-1ac7-30f1-d03541b64bb8"), "Signing in again builds a token that includes Finance-RW, and the existing permissions then let him in.", 4, "What is the fix?", new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e") },
                    { new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"), "A running service keeps its logon session, so the saved password is only used at the next start", new Guid("902f20ea-2bce-9172-f1a1-7094be61b162"), "The service signed in before the rotation and kept running. The restart was the first time Windows tried the saved password again.", 3, "The password was rotated a week ago. Why did the service only fail on Sunday?", new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e") },
                    { new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"), "The PC's secure channel is broken, as its machine account password doesn't match the domain's", new Guid("ef4a8924-b2db-87c9-fdb8-2ff9ce857314"), "The computer proves its identity to the domain with its own account password. When the two sides disagree, the secure channel fails and so do domain sign-ins.", 1, "What does this error mean?", new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417") },
                    { new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"), "Sign in with a local administrator account", new Guid("2fc27848-f026-ebb2-33e6-7465f7973cbe"), "A local account is checked against the PC's own account database, so it doesn't need the secure channel.", 2, "Domain sign-ins fail on this PC. How do you get in to repair it?", new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417") },
                    { new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"), "Run gpupdate /force on a sales PC, then confirm gpresult /r lists the GPO as applied", new Guid("b130bf08-00a0-3481-255e-44d08a5d5106"), "Checking on a real sales PC as a sales user tests the exact path that was failing.", 5, "How do you confirm the fix?", new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f") },
                    { new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"), "The password saved in the service no longer matches the account's password in the domain", new Guid("885881fb-4c09-800f-35f9-07d55d828add"), "The event names the configured password as the failure, and the account's password was rotated last week.", 2, "Event 7038 says the service was unable to log on as CORP\\svc-sync with the currently configured password, because the user name or password is incorrect. What does this mean?", new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e") },
                    { new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"), "Find what is still sending the old password before unlocking the account again", new Guid("7b6ecbdc-658e-96dc-9be0-7d08b2db578c"), "A lockout on a timer has a machine behind it. Until that source is found, each unlock only lasts until its next attempt.", 1, "The help desk wants to unlock the account a seventh time. What should happen first?", new Guid("652a4793-984d-cf69-18d2-336dda12226c") },
                    { new Guid("b70a712e-865b-04bf-a392-cb31612c6602"), "Move the service to a group managed service account, whose password is handled automatically", new Guid("6c8b289f-5995-2419-ac5b-48791a1d059e"), "With a gMSA, Active Directory rotates the password and the server retrieves it, so there's no saved password to go stale.", 5, "How do you stop the next password rotation from causing the same outage?", new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e") },
                    { new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"), "Connect with mstsc to localhost from the console, then test port 3389 from another PC", new Guid("e10161e5-5290-931d-f2f0-111a27a7276f"), "A local connection proves the RDP service works. Test-NetConnection with -Port 3389 from elsewhere then tests the path to it.", 2, "qwinsta shows rdp-tcp in the Listen state. How do you tell a server fault from a network fault?", new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24") },
                    { new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"), "Sign in at the console and run qwinsta to check for an rdp-tcp listener in the Listen state", new Guid("fb77ffe1-752d-5c10-e79a-fc85e1c6f77b"), "This shows whether the server is accepting RDP connections at all, which splits the problem into listener and network.", 1, "The server is running. Where do you start?", new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24") },
                    { new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"), "On RECEPTION-PC02, read the 4625 events to see which logon type is failing", new Guid("e019d919-1d05-1d0b-872b-9455ef6478cb"), "Event 4625 on the source computer records each failed logon and its logon type, which shows what kind of thing is sending the password.", 3, "Event 4740 shows Caller Computer Name: RECEPTION-PC02, a shared front-desk PC Dana covered last week. What next?", new Guid("652a4793-984d-cf69-18d2-336dda12226c") },
                    { new Guid("d243f093-453e-c659-371e-cbfea2afe06f"), "Run gpresult /h report.html as the user and read which GPOs were applied or denied", new Guid("0defef44-8c4d-54f1-0d45-3604e75f712e"), "The report shows what the client actually processed and the reason for anything it skipped, so you aren't guessing from the server side.", 1, "What do you do first on an affected sales PC?", new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f") },
                    { new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"), "Run Dism /Online /Cleanup-Image /AnalyzeComponentStore, then /StartComponentCleanup", new Guid("8983e9e5-8453-c9b6-e302-fb05ba4cfb24"), "The analysis reports what can be reclaimed, and the cleanup removes superseded component versions in the supported way.", 3, "How do you safely reduce the WinSxS folder?", new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3") },
                    { new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"), "Enter the new password on the service's Log On tab, then start the service", new Guid("fd7fa08d-1091-c8e6-a313-ecf9fac7e289"), "This brings the saved password back in line with the domain, which is the mismatch the event described.", 4, "What is the right fix?", new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e") },
                    { new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"), "Fix or remove the task's saved credentials, unlock the account, then watch for new 4740 events", new Guid("977598cc-6515-5a8f-fbd8-4a4767a783e5"), "Removing the source stops the bad attempts. Watching for further lockout events proves that there wasn't a second source.", 5, "You find a report-export task running as Dana. How do you finish the ticket?", new Guid("652a4793-984d-cf69-18d2-336dda12226c") },
                    { new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"), "The Security log on the PDC emulator, filtered to event 4740 for Dana's account", new Guid("4b7e9f1f-148d-12ad-d30b-399bd597dafb"), "Lockouts are processed on the PDC emulator, and event 4740 there records the computer that the failed attempts came from.", 2, "Where do you look to find out where the bad passwords are coming from?", new Guid("652a4793-984d-cf69-18d2-336dda12226c") },
                    { new Guid("ef96842e-8c24-101f-8659-f94681f081eb"), "A scheduled task on that PC saved under Dana's account with her old password", new Guid("2137810b-ea64-6631-70f6-84bf2a2dddff"), "Logon type 4 is a batch logon, which is how scheduled tasks sign in. The regular interval matches a task trigger.", 4, "The 4625 events for Dana's account on that PC all show Logon Type 4, about 20 minutes apart. What is the likely cause?", new Guid("652a4793-984d-cf69-18d2-336dda12226c") },
                    { new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"), "Run Test-ComputerSecureChannel -Repair -Credential with a domain account allowed to reset it", new Guid("7233e2c4-b9d0-3872-eb32-85d612ecc73d"), "This resets the machine account password on both sides in place, usually with no restart and with the computer object left intact.", 4, "The test returns False. What is the least disruptive fix?", new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417") },
                    { new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"), "Enable the Remote Desktop firewall rule group for the profiles the server uses", new Guid("d37af0a2-48d2-4e13-9851-8f6a9706b5be"), "The built-in rules allow exactly the traffic RDP needs. Get-NetFirewallRule -DisplayGroup \"Remote Desktop\" | Set-NetFirewallRule -Enabled True does it in one line.", 4, "In Windows Defender Firewall on the server, the rule Remote Desktop - User Mode (TCP-In) is disabled. What is the fix?", new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24") },
                    { new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"), "The System event log, in Service Control Manager events such as 7038 and 7041", new Guid("258e3b17-a03a-6d5f-cefa-02bfe3b7c431"), "Error 1069 is generic. The Service Control Manager event states which account failed to log on and why.", 1, "Where do you find the exact reason for the failure?", new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e") }
                });

            migrationBuilder.InsertData(
                table: "Distractors",
                columns: new[] { "Id", "Explanation", "SequentialStepId", "Text" },
                values: new object[,]
                {
                    { new Guid("01edf9ce-9808-5bc0-217b-1020c96759cb"), "Nothing in a 1722 error points to Kerberos keys, and resetting krbtgt is a major change.", new Guid("46baf750-afaa-a6df-25f8-be25c4030396"), "The two DCs disagree about a password, so the krbtgt account needs to be reset" },
                    { new Guid("026f0fff-2bf4-56a7-c55a-bb909bb8bf71"), "System File Checker repairs protected system files. It doesn't remove superseded components.", new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"), "Run sfc /scannow" },
                    { new Guid("06be2beb-4266-8173-e3af-47c8e1cece4a"), "A domain-joined PC with no such account set up has nothing to check it against.", new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"), "Sign in with the user's Microsoft account" },
                    { new Guid("0b7f8656-6f58-7fcc-61cb-2b372875f5d7"), "It describes how the service was installed, not why it can't sign in today.", new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"), "The vendor's installation log from two years ago" },
                    { new Guid("0e01cc69-71fc-0423-3896-e506edff4f6f"), "Rejoining works, but it isn't the only way, and it costs two restarts for a problem one command can fix.", new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"), "Move the PC to a workgroup, restart, join the domain again and restart once more, as that's the only supported repair" },
                    { new Guid("10cc3734-0031-aa3e-e984-75315db1327f"), "Policy is processed on the client. Refreshing a DC's own policy does nothing for a sales PC.", new Guid("d243f093-453e-c659-371e-cbfea2afe06f"), "Run gpupdate /force on the domain controller that holds the PDC emulator role, and then wait 90 minutes" },
                    { new Guid("121baa44-15cc-bca2-7b38-d5d9fd44b2a3"), "The message says logon failure. The restart only exposed it, and the Setup log won't explain a logon.", new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"), "The Setup log, because the fault appeared straight after patching and a failed update is the likeliest cause" },
                    { new Guid("125dd033-06f4-867e-a392-b6e4516811bc"), "This removes lockout protection from her account and leaves the faulty task running.", new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"), "Create a fine-grained password policy with a lockout threshold of 0 and apply it to Dana, so her account can't lock" },
                    { new Guid("132e813f-8b42-bd31-f944-ba2404333338"), "DNS logs record name resolution, not authentication failures.", new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"), "The DNS Server log on the nearest domain controller" },
                    { new Guid("19e9e6b0-b6c3-0cc4-30d6-daa6bf43997a"), "The stale token is on his own session, so a server restart interrupts everyone and fixes nothing.", new Guid("981d38df-be64-d403-71f2-931cdbd8145e"), "Restart FILE-SRV01, so that every connected user is forced to collect fresh group memberships from the domain" },
                    { new Guid("1c5f4029-fba9-71b6-07ce-ac42285db43a"), "The console worked all along. The ticket is about remote sessions.", new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"), "Connect once from the Hyper-V console and close the ticket, since the console is the more secure way in anyway" },
                    { new Guid("1ce8f7a7-7f23-abe0-e004-86ea7a853b91"), "The copy goes out of date immediately, and it doubles the sensitive data to protect.", new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"), "Make a copy of the folder for the auditors" },
                    { new Guid("21bab13a-e1b3-a286-a6e1-8620eac9b14b"), "Windows never fetches a new password for an ordinary account. An administrator has to enter it.", new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"), "Windows keeps service passwords in a cache for seven days, then asks a domain controller for the new one" },
                    { new Guid("2266b57c-3e84-a145-ef56-302b5d5f15de"), "This refreshes Group Policy on one computer. It doesn't report on replication between DCs.", new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"), "gpupdate /force" },
                    { new Guid("28d290c1-67d7-3438-b582-c4cb284ed62c"), "Neither set overrides the other. Each can only narrow the result.", new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"), "Share permissions override NTFS permissions" },
                    { new Guid("29c6531b-493d-eff9-ca31-4356b01d2e5a"), "That is event 7041, whose text says the user has not been granted the requested logon type.", new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"), "The account lacks the Log on as a service right" },
                    { new Guid("2a4e9990-8828-0c43-7c62-0a24d28bbd25"), "There's no such delay. The permission is on the group, and his token doesn't carry the group.", new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"), "NTFS permissions take up to 24 hours to reach new group members" },
                    { new Guid("2a9f6e09-0e21-c0ae-e952-f77a02acf2a6"), "Password changes replicate urgently, within seconds or minutes.", new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"), "Password changes take about a week to replicate between domain controllers" },
                    { new Guid("2aba4a6c-5b26-56ce-5f0b-42a688fed4b3"), "This reverses a security control and breaks anything already updated to the new password.", new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"), "Set svc-sync's password in Active Directory back to the old value, without involving the security team" },
                    { new Guid("2d179647-ae83-d380-cfa3-7ba38d7cba66"), "The PC then has no account at all, and a new object gets a new SID and loses its group memberships.", new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"), "Delete the computer object in Active Directory, then restart" },
                    { new Guid("2f8a2390-3034-eca8-d9ca-d137fcdb052b"), "Lockout events are security events written by domain controllers, and her laptop may not be the source.", new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"), "The Application log on Dana's laptop" },
                    { new Guid("32fb73e2-68dc-f87f-12c2-dca08026b331"), "Event 7038 has separate wording for that, saying the account is currently disabled.", new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"), "The account has been disabled" },
                    { new Guid("33f7363d-7280-14b3-8bdb-d430c1e193d4"), "If the policy can't be read, it fails the same way on every restart.", new Guid("d243f093-453e-c659-371e-cbfea2afe06f"), "Restart the PC a few times" },
                    { new Guid("373926fc-084c-96c4-b0cd-a3f878f3453a"), "The localhost session used that service a moment ago, so it's running.", new Guid("08877fab-aa11-4280-5064-4ea65ce68431"), "The Remote Desktop Services service has stopped" },
                    { new Guid("3dc77a10-67e8-99d0-6775-9c29a0b345cb"), "An expired user password gives a prompt to change it, and it wouldn't affect every user on one PC.", new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"), "The user's domain password has expired" },
                    { new Guid("3de6449a-2db2-ed33-4757-b78d6e8c41df"), "That folder holds the registry hives. Removing them stops Windows from starting.", new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"), "Delete the contents of C:\\Windows\\System32\\config" },
                    { new Guid("3e47f000-51e2-747f-63ee-eb4803b643a1"), "The stale credential is wrong against any new password, so the lockouts continue.", new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"), "Reset Dana's password to something new and have her sign in again" },
                    { new Guid("41ced850-5416-a30b-49cd-4d8cea696e3e"), "Local System has full control of the server and none of the account's permissions on other systems.", new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"), "Switch the service to Local System" },
                    { new Guid("44313827-134c-af1a-6290-1179afa326ad"), "Silence isn't evidence, and it leaves users without the drive in the meantime.", new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"), "Assume it worked if nobody in Sales has called back about the missing drive by the end of the week" },
                    { new Guid("4444ff9c-0568-c3b1-3d8f-9c6b83384863"), "This leaves verification to users and a day's delay, when one command answers it now.", new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"), "Ask the new users to try again tomorrow, and reopen the ticket if any of them still can't sign in at the branch" },
                    { new Guid("45b5ee97-6c6d-8b3c-eefd-adb8ffc698fc"), "Policy would fail, but only as a side effect. It doesn't test the channel itself.", new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"), "Run gpupdate /force and read through the output for computer policy errors, then compare it with a working PC" },
                    { new Guid("46dc63ac-6a3d-6c60-fe3c-98cb2af66220"), "An expired password produces a credentials message after connecting, not this error.", new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"), "Check that the administrators' passwords haven't expired" },
                    { new Guid("4e36de6a-0867-e76c-a635-81710ea0cbee"), "That would affect many computers at once, not one reverted virtual desktop.", new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"), "The domain controller's certificate has expired" },
                    { new Guid("4e8947af-7831-58aa-1bc3-2f09b7817d4d"), "A reply shows the network path works. It says nothing about whether the domain trusts this PC.", new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"), "Ping the domain controller's IP address" },
                    { new Guid("516c2eeb-9241-e8bc-69f7-d383aa4bc09f"), "WinSxS is the component store. Deleting it can leave the server unable to boot or install updates.", new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"), "Delete the WinSxS folder" },
                    { new Guid("5286de5a-30dd-7d4f-b5e8-ba871ee65bd7"), "Local admin rights don't grant the computer account access to the GPO, and they create a security problem.", new Guid("1823c892-3611-380b-80d0-0441fa9616af"), "Add every sales user to the local Administrators group on their PC, so that their sessions are able to read the policy" },
                    { new Guid("5316b57b-b479-0236-e238-f9e4cf8dcdcb"), "A delayed start helps when the domain can't be contacted at boot. Here the password is wrong.", new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"), "Change the startup type to Automatic (Delayed Start)" },
                    { new Guid("5de27595-18d0-b906-bd41-1545b5aed64b"), "The alternate is only used when the preferred server doesn't answer, and 8.8.8.8 answers that the name doesn't exist.", new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"), "Keep 8.8.8.8 as preferred and add an internal DNS server as the alternate, so that internet and domain names both resolve" },
                    { new Guid("5e4dcde4-3e02-6eed-ba75-a25fa8d35c98"), "Global security groups work for security filtering.", new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"), "Sales-Users is a global group, and filtering needs a domain local group" },
                    { new Guid("61014c73-43d8-bf04-4b37-3901476a138a"), "The caller would then be the mail or sync server, not the front-desk PC.", new Guid("ef96842e-8c24-101f-8659-f94681f081eb"), "Dana's phone is sending the old password to the mail server" },
                    { new Guid("63467b33-c6ff-35af-09de-8fd3d4c6a5b5"), "No rule allows 3390 either, so the connection is still blocked.", new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"), "Change the RDP port to 3390 in the registry so that the sessions avoid the rule that was disabled" },
                    { new Guid("6442296a-b949-6877-1ec4-496db713d12d"), "That reflects local services and events, not whether partitions are up to date.", new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"), "Look for a green status icon in Server Manager" },
                    { new Guid("6c33894f-be3c-922c-b398-56ed7a68a2e3"), "Forcing a sync that can't connect just repeats the same failure.", new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"), "Run repadmin /syncall over and over until the error stops appearing in the output" },
                    { new Guid("6f205e39-6d7f-305e-f04d-19e91e9e8816"), "It bypasses the group, gives him more than his colleagues have, and leaves a one-off entry to maintain.", new Guid("981d38df-be64-d403-71f2-931cdbd8145e"), "Grant Tomas's own account Full Control on the folder" },
                    { new Guid("7222a94d-1610-01ee-ecba-49e686f93dfe"), "That grants Apply as well as Read, so the drive maps for everyone in the Staff OU again.", new Guid("1823c892-3611-380b-80d0-0441fa9616af"), "Put Authenticated Users back into Security Filtering" },
                    { new Guid("7682ba45-8611-376a-d7ef-55621e92e0de"), "An administrator outside Sales-Users shouldn't get it, so that test can only mislead.", new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"), "Confirm that an administrator's PC gets the drive" },
                    { new Guid("77f87569-0536-7eb9-3182-74a7f7b3d034"), "The GPO is linked to that OU directly. Block Inheritance only stops GPOs from higher up.", new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"), "Whether Block Inheritance is set on the Staff OU" },
                    { new Guid("794069d1-42c9-0bee-33b2-cb8b22442ace"), "There's no such rule. Clients process GPOs regardless of which server version created them.", new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"), "Whether the PC runs a newer Windows build than the domain controllers, since newer clients skip GPOs made on older servers" },
                    { new Guid("7a5a885a-bfc5-9509-a74e-6d6f03072b5c"), "This opens the Finance folder to every user on the network.", new Guid("981d38df-be64-d403-71f2-931cdbd8145e"), "Add Everyone with Full Control in NTFS" },
                    { new Guid("7a93f082-e182-5e4c-c3fa-469d21fd9f25"), "The event describes a logon with a password. It says nothing about the program file.", new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"), "An update replaced the service's program file, and the Service Control Manager can no longer verify its signature" },
                    { new Guid("7ac06d6f-fbef-142e-4cde-a42b2e787e15"), "A permissions failure is reported as error 5 or 8453, access denied.", new Guid("46baf750-afaa-a6df-25f8-be25c4030396"), "BR-DC02 has no permission to replicate" },
                    { new Guid("8318175a-fd10-c9d5-9b02-034e65e92fd7"), "A restart interrupts file shares that are working and won't undo a configuration change.", new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"), "Restart the server from Hyper-V Manager" },
                    { new Guid("842585f2-abc0-b923-5172-826c870d7316"), "Group membership doesn't update a saved password, and this grants enormous privileges.", new Guid("b70a712e-865b-04bf-a392-cb31612c6602"), "Add svc-sync to the Domain Admins group" },
                    { new Guid("85c5c75f-701c-6015-bb13-dd7659a24e75"), "This takes the website down, and the logs already on disk still fill the drive.", new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"), "Stop the World Wide Web Publishing Service so no more logs can be written while you decide what to do" },
                    { new Guid("88e73e5a-241d-7aa5-5870-50e5eaa97e31"), "Filtering to other groups is supported. It just needs computers to keep Read access.", new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"), "A GPO with no Authenticated Users entry in Security Filtering is treated as disabled, and every client in the domain skips it" },
                    { new Guid("8bf9547d-079c-d51b-0e60-866af04de827"), "Windows tracks those files. Removing them by hand breaks servicing and can stop the server booting.", new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"), "Delete its oldest subfolders in File Explorer" },
                    { new Guid("8dfa37d3-41b2-131b-7a40-0b017eaff031"), "This breaks her sign-in and everything else tied to the name, to avoid editing one task.", new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"), "Rename Dana's account so the task can no longer find it" },
                    { new Guid("8fd28a34-3658-ae71-8014-5d11cb8a68d2"), "This hides the fault and leaves a GPO that still doesn't work for the next person.", new Guid("d243f093-453e-c659-371e-cbfea2afe06f"), "Map the drive by hand on each sales PC" },
                    { new Guid("91371ded-17cd-068b-6542-c78e2eb4fa82"), "Turning it off briefly can confirm a diagnosis, but leaving it off removes protection from every port.", new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"), "Turn Windows Firewall off on all profiles and leave it off" },
                    { new Guid("9177181d-8928-75c7-dfe3-6a49339b3c34"), "This leaves the server unpatched to save a small amount of space.", new Guid("94417086-b2e1-926e-f906-10354c29eed4"), "Disable Windows Update to stop WinSxS growing" },
                    { new Guid("91bb2483-a33d-5901-d8f0-468166425b53"), "A leftover saved credential is far more likely, and reimaging destroys the evidence either way.", new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"), "Reimage RECEPTION-PC02, because a PC that keeps sending bad passwords has clearly been infected by malware" },
                    { new Guid("942c311e-b80e-12e7-4b16-31d2a105be87"), "His colleagues can open the same folder, so the file system isn't at fault.", new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"), "In the output of chkdsk on the data volume, in case file system errors have damaged the permissions on that folder" },
                    { new Guid("9484f77a-3b17-66dd-4cff-27dc974056f4"), "Losing the ability to uninstall a bad update matters on a production server.", new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"), "There is none, so always use it" },
                    { new Guid("94a7e783-5867-8643-5841-407db77890d6"), "Group membership is checked after the connection is made, so the port test would succeed.", new Guid("08877fab-aa11-4280-5064-4ea65ce68431"), "The administrators aren't members of the Remote Desktop Users group" },
                    { new Guid("9504817a-86de-bed4-2de7-6f616fbd4228"), "Name resolution isn't mentioned anywhere in this error.", new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"), "The DNS Server log on a domain controller" },
                    { new Guid("95c47825-c22f-0ce0-4aa1-0a033ad2313e"), "The task still holds the old password, so the account locks again at its next run.", new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"), "Unlock the account and close the ticket" },
                    { new Guid("984786af-f85a-f610-4ebf-060be174f81d"), "Expiry isn't involved. The password was already changed, and something kept the old one.", new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"), "Set Dana's password to never expire" },
                    { new Guid("99e17c62-3b9c-21c6-8728-30539859c579"), "Rebuilding a DC before diagnosing it is drastic, and the new one would hit the same fault.", new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"), "dcpromo on BR-DC02, to demote it and promote it again so that it pulls down a completely fresh copy of the directory" },
                    { new Guid("9a64a5fd-4f33-6220-0540-891969df66f9"), "Ping uses ICMP and already works. It can't show whether TCP 3389 is reachable.", new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"), "Ping the server from an administrator's PC" },
                    { new Guid("9c686426-799e-3cd1-8757-357bd6a166a4"), "Routing is fine, because ping and file shares work. The question is one TCP port.", new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"), "Run tracert to the server and count the hops along the way to see whether the route has changed since yesterday" },
                    { new Guid("a00ac0d4-3c12-18c7-a439-4c55dd980bc1"), "This opens the whole server to fix one port.", new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"), "Add a rule that allows all inbound TCP ports" },
                    { new Guid("a7abcf02-20b6-d4db-2502-12c06dfdf07b"), "Every administrator fails the same way after a server change, so the clients aren't the cause.", new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"), "Delete each administrator's saved .rdp file and clear the cached credentials in Credential Manager on their workstations" },
                    { new Guid("a89c08ba-65e3-56a5-60f5-dbafb33e4691"), "User passwords were never the problem. The mismatch is on the computer's account.", new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"), "Reset the user's password in Active Directory" },
                    { new Guid("aab06f51-5fbd-627f-828b-da4e0ae8ec5f"), "A WMI filter on another GPO has no effect on this one.", new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"), "The WMI filter on the Default Domain Policy" },
                    { new Guid("afe553ed-32d2-7fee-1a44-e315c3ae8c90"), "This guesses at the source. The lockout event names it directly.", new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"), "The System log on each file server Dana uses, starting with the one she opens most often during the day" },
                    { new Guid("b0a3677e-d2e3-d465-7341-196900a74d41"), "NTFS permissions apply to every kind of access. It's share permissions that only apply over the network.", new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"), "NTFS permissions are skipped for network access, and they only apply to people signed in at the server itself" },
                    { new Guid("b1467ee5-a030-53e4-15e2-98dbcfa77342"), "Without finding the cause, the new space fills the same way and the alert returns.", new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"), "Extend the C: volume straight away and close the alert, since every server needs a bigger disk as it gets older" },
                    { new Guid("b7c4135f-dcc0-e164-6f7e-11956b1a2419"), "That lists shadow copies for restoring files. It has nothing to do with permissions.", new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"), "In the folder's Previous Versions tab" },
                    { new Guid("bc715a56-7ffc-a696-2437-a491be31a79e"), "Denying Full Control denies every permission, including Read, and Deny beats any Allow.", new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"), "Add a Deny Full Control entry for the auditors on the share" },
                    { new Guid("c2227fbb-9e87-b2e1-b8b3-a6523674aac0"), "An unchanging password on a service account is exactly the risk rotation exists to remove.", new Guid("b70a712e-865b-04bf-a392-cb31612c6602"), "Set the password to never expire and exclude the account from every future rotation" },
                    { new Guid("c7d07363-9239-0c0c-40b3-45080ca4561b"), "This lists which DCs hold the operations master roles, not whether they are replicating.", new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"), "netdom query fsmo" },
                    { new Guid("c8450ca1-8111-3521-f02c-798ba267e769"), "Changing ownership of the component store breaks servicing, and the store already compresses unused components.", new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"), "Take ownership of the folder from TrustedInstaller, then turn on NTFS compression for it from the Properties page" },
                    { new Guid("cbd24427-9e0f-5b5c-a36a-3e7383f9b60c"), "That would be a network logon, type 3, and the caller would be her laptop.", new Guid("ef96842e-8c24-101f-8659-f94681f081eb"), "A mapped network drive on Dana's own laptop is reconnecting with the credentials it saved before Monday" },
                    { new Guid("cc51572b-4ee4-634b-c55b-3ba15f072712"), "Ping worked while replication was failing, so it can't show that anything changed.", new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"), "Check that BR-DC02 answers a ping from HQ-DC01" },
                    { new Guid("ce58e1c0-39d7-7a23-b9fc-a8a1496752b8"), "It's the reverse. Access is limited to what both sets allow.", new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"), "Whichever of the two is more generous applies" },
                    { new Guid("d07e1f81-0d42-3083-7b94-011d6b59afaa"), "User groups are valid for filtering user settings. The missing piece is Read for computers.", new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"), "Security filtering only accepts computer groups" },
                    { new Guid("d0aa4331-fdbd-f70e-7da0-ccb47d996683"), "The PC can already find the domain. What's wrong is what happens when it gets there.", new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"), "Run ipconfig /flushdns and try the sign-in again" },
                    { new Guid("d0ed2fca-b16e-972d-79ab-5c89e6f9cbab"), "The link was enabled throughout the fault, so it proves nothing.", new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"), "Check in the console that the GPO link is enabled" },
                    { new Guid("d2b6bf45-9952-75a1-8571-2413a757d2f8"), "The logs are needed for troubleshooting and security investigations. Rotate them instead.", new Guid("94417086-b2e1-926e-f906-10354c29eed4"), "Turn off IIS logging on the server completely, because nobody has read those log files in four years" },
                    { new Guid("d50e48fb-26d1-5a9e-6677-569260a744bf"), "Anyone who can read the file can then use the account.", new Guid("b70a712e-865b-04bf-a392-cb31612c6602"), "Save the password in a text file on the server" },
                    { new Guid("d5d8ca91-72f8-b942-6bd9-b363c19886e4"), "Restarts don't delete logs, and they add downtime for no benefit.", new Guid("94417086-b2e1-926e-f906-10354c29eed4"), "Schedule a restart every night" },
                    { new Guid("d79169b8-f04a-de8d-b722-28586525e447"), "Enforced changes precedence over other GPOs. It can't make an unreadable GPO readable.", new Guid("1823c892-3611-380b-80d0-0441fa9616af"), "Set the GPO link to Enforced" },
                    { new Guid("d9a9eb7b-c062-b903-3aff-1b414fe8906c"), "Shares refer to groups by their identifier, so membership changes need nothing done to the share.", new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"), "The share has to be removed and created again every time the membership of one of its groups changes" },
                    { new Guid("da3f1f0a-d2dd-9d7d-23d0-6ff7f84ee4cd"), "It only affects superseded component versions. Roles, features and data are untouched.", new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"), "It resets Windows to its original image and removes installed roles" },
                    { new Guid("daced8e7-d00a-55e1-1174-f3786b1b5e4d"), "Member servers and PCs don't replicate the directory. Only domain controllers do.", new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"), "Replication is failing between FILE-SRV01 and his PC" },
                    { new Guid("dad0608e-ebbf-4967-05c4-5243761fc7c5"), "This weakens password-guessing protection for every account to hide one fault.", new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"), "Raise the lockout threshold in the Default Domain Policy so that accounts take far longer to lock out" },
                    { new Guid("dba1e3b0-591b-6baa-1a27-676f1672e7b8"), "Ping answered throughout the fault, so it shows nothing about RDP.", new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"), "Confirm that the server still answers ping" },
                    { new Guid("dbdc8059-5ff4-992d-e6ed-6f1d86d1c736"), "The source DC is serving headquarters normally, and this adds an outage without a diagnosis.", new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"), "Restart HQ-DC01 during the working day" },
                    { new Guid("de21cb6b-8375-2700-e512-ae572b02ba48"), "Every domain controller holds the same machine account password, so none will accept this PC.", new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"), "Keep retrying until a different domain controller answers" },
                    { new Guid("dfba8f92-b95b-24af-b050-0e386c4a4349"), "Administrative RDP doesn't use that role, and reinstalling skips the diagnosis.", new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"), "Remove the Remote Desktop Services role and add it again" },
                    { new Guid("e590888e-204d-5e58-84c8-46b73ae9f273"), "The connection can't be attempted until the name resolves, and the DNS test already found the fault.", new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"), "Open TCP port 135 on the branch firewall" },
                    { new Guid("e5feb548-adfb-2d7c-8caf-88e13dc17290"), "Database faults produce different errors. This one says the other server couldn't be reached.", new Guid("46baf750-afaa-a6df-25f8-be25c4030396"), "The directory database on BR-DC02 is corrupt" },
                    { new Guid("e6c0db40-1e4b-574a-59ed-996022549221"), "A restart frees little, interrupts the site, and may not complete cleanly on a full disk.", new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"), "Restart the server to clear out temporary files" },
                    { new Guid("e97b8880-d045-33a4-3685-1c5cbac1beb7"), "A domain admin is still a domain account, and it needs the same broken channel to be authenticated.", new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"), "Sign in as a member of Domain Admins, since that group's rights let it bypass the secure channel" },
                    { new Guid("ea70c14a-b72d-918e-81f1-aa5566eb1e17"), "Despite the wording, this message is about the computer's own account, not a trust between domains.", new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"), "An administrator has removed the forest trust to a partner organization, so its accounts can no longer sign in here" },
                    { new Guid("eaefbf94-a67d-9d33-cbf5-cd4011b9917a"), "Nothing ties the profile to the failed attempts, and this risks losing her data.", new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"), "Delete Dana's roaming profile from the profile server" },
                    { new Guid("ec2d156e-3450-c12f-47fc-44be525497dc"), "This risks out-of-memory failures and removes the ability to capture a crash dump.", new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"), "Turn off the page file and delete pagefile.sys" },
                    { new Guid("eeff9151-035e-114c-a4b8-b47230a390ed"), "Bad user passwords lock user accounts. They don't affect the computer's own account.", new Guid("81b4863b-3305-e700-519f-7b935733ab79"), "Too many users typed wrong passwords on that PC" },
                    { new Guid("efb5fb9e-3a68-978c-7dde-2a305c369a36"), "The computer starts its own password changes, and Active Directory doesn't expire machine passwords for being offline.", new Guid("81b4863b-3305-e700-519f-7b935733ab79"), "The VM had been off for more than 30 days, so Active Directory expired its machine account password" },
                    { new Guid("f24eafe3-5b93-cbb6-6dcc-f2165548a2a7"), "That takes a working front-desk PC off the domain and tells you nothing about the cause.", new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"), "Disable the RECEPTION-PC02 computer account in Active Directory" },
                    { new Guid("f52ff85e-dabb-6c0c-5d51-8f0404c13381"), "The secure channel is based on the machine account password, not the address.", new Guid("81b4863b-3305-e700-519f-7b935733ab79"), "The revert gave the VM a different IP address" },
                    { new Guid("f56c914d-2562-1ad4-8462-172ce4ca25ad"), "Windows updates don't change the credentials saved for services.", new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"), "The update reset every service password" },
                    { new Guid("f5b9c24e-359e-de41-e0e9-9332c38cde13"), "Certificates are exchanged after the TCP connection opens, so a bad one can't fail a port test.", new Guid("08877fab-aa11-4280-5064-4ea65ce68431"), "The server's Remote Desktop certificate has expired, which stops the TCP connection from being opened in the first place" },
                    { new Guid("f5deba38-7e3b-ac52-9142-d86c4c8870cc"), "Domain policy doesn't hold permissions for individual folders.", new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"), "In the Default Domain Policy" },
                    { new Guid("f5fef0f3-daab-7574-d322-6b593e1b037a"), "Installed updates stay installed. What's lost is the ability to roll them back.", new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"), "It clears the update history, so the server has to download and reinstall every update released since it was built" },
                    { new Guid("f99cdf49-7d17-f511-4405-e77d01a159cc"), "Nothing has been tested, and the hardening change may reapply at its next run.", new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"), "Email the administrators that it should be fixed" },
                    { new Guid("fc606c9f-5d25-0602-192c-35e0176198b1"), "Typing at the keyboard is an interactive logon, type 2, and people don't fail on a 20-minute timer.", new Guid("ef96842e-8c24-101f-8659-f94681f081eb"), "Someone at the front desk keeps typing Dana's password incorrectly" },
                    { new Guid("fd4a27e0-4455-94c0-5a03-bc0f02ec094b"), "DCs find each other through several records in the _msdcs zone, which a single hosts entry can't replace.", new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"), "Add HQ-DC01's name and address to the hosts file on BR-DC02" },
                    { new Guid("fe54988e-fb68-28b0-4529-033b52c77654"), "Seizing roles is for a role holder that is permanently gone. It isn't a replication fix.", new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"), "Seize the operations master roles onto BR-DC02" },
                    { new Guid("ff92890f-dd88-8a76-e6f5-81ca324651c0"), "They could still reach the original share, where NTFS Modify would let them change files.", new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"), "Create a second, read-only share for the same folder and grant the auditors Modify in NTFS so they can open it" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("01edf9ce-9808-5bc0-217b-1020c96759cb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("026f0fff-2bf4-56a7-c55a-bb909bb8bf71"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("06be2beb-4266-8173-e3af-47c8e1cece4a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0b7f8656-6f58-7fcc-61cb-2b372875f5d7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0e01cc69-71fc-0423-3896-e506edff4f6f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("10cc3734-0031-aa3e-e984-75315db1327f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("121baa44-15cc-bca2-7b38-d5d9fd44b2a3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("125dd033-06f4-867e-a392-b6e4516811bc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("132e813f-8b42-bd31-f944-ba2404333338"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("19e9e6b0-b6c3-0cc4-30d6-daa6bf43997a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1c5f4029-fba9-71b6-07ce-ac42285db43a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1ce8f7a7-7f23-abe0-e004-86ea7a853b91"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("21bab13a-e1b3-a286-a6e1-8620eac9b14b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2266b57c-3e84-a145-ef56-302b5d5f15de"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("28d290c1-67d7-3438-b582-c4cb284ed62c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("29c6531b-493d-eff9-ca31-4356b01d2e5a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2a4e9990-8828-0c43-7c62-0a24d28bbd25"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2a9f6e09-0e21-c0ae-e952-f77a02acf2a6"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2aba4a6c-5b26-56ce-5f0b-42a688fed4b3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2d179647-ae83-d380-cfa3-7ba38d7cba66"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2f8a2390-3034-eca8-d9ca-d137fcdb052b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("32fb73e2-68dc-f87f-12c2-dca08026b331"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("33f7363d-7280-14b3-8bdb-d430c1e193d4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("373926fc-084c-96c4-b0cd-a3f878f3453a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3dc77a10-67e8-99d0-6775-9c29a0b345cb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3de6449a-2db2-ed33-4757-b78d6e8c41df"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3e47f000-51e2-747f-63ee-eb4803b643a1"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("41ced850-5416-a30b-49cd-4d8cea696e3e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("44313827-134c-af1a-6290-1179afa326ad"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4444ff9c-0568-c3b1-3d8f-9c6b83384863"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("45b5ee97-6c6d-8b3c-eefd-adb8ffc698fc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("46dc63ac-6a3d-6c60-fe3c-98cb2af66220"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4e36de6a-0867-e76c-a635-81710ea0cbee"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4e8947af-7831-58aa-1bc3-2f09b7817d4d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("516c2eeb-9241-e8bc-69f7-d383aa4bc09f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5286de5a-30dd-7d4f-b5e8-ba871ee65bd7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5316b57b-b479-0236-e238-f9e4cf8dcdcb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5de27595-18d0-b906-bd41-1545b5aed64b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5e4dcde4-3e02-6eed-ba75-a25fa8d35c98"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("61014c73-43d8-bf04-4b37-3901476a138a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("63467b33-c6ff-35af-09de-8fd3d4c6a5b5"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6442296a-b949-6877-1ec4-496db713d12d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6c33894f-be3c-922c-b398-56ed7a68a2e3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6f205e39-6d7f-305e-f04d-19e91e9e8816"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7222a94d-1610-01ee-ecba-49e686f93dfe"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7682ba45-8611-376a-d7ef-55621e92e0de"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("77f87569-0536-7eb9-3182-74a7f7b3d034"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("794069d1-42c9-0bee-33b2-cb8b22442ace"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7a5a885a-bfc5-9509-a74e-6d6f03072b5c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7a93f082-e182-5e4c-c3fa-469d21fd9f25"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7ac06d6f-fbef-142e-4cde-a42b2e787e15"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8318175a-fd10-c9d5-9b02-034e65e92fd7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("842585f2-abc0-b923-5172-826c870d7316"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("85c5c75f-701c-6015-bb13-dd7659a24e75"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("88e73e5a-241d-7aa5-5870-50e5eaa97e31"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8bf9547d-079c-d51b-0e60-866af04de827"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8dfa37d3-41b2-131b-7a40-0b017eaff031"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8fd28a34-3658-ae71-8014-5d11cb8a68d2"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("91371ded-17cd-068b-6542-c78e2eb4fa82"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9177181d-8928-75c7-dfe3-6a49339b3c34"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("91bb2483-a33d-5901-d8f0-468166425b53"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("942c311e-b80e-12e7-4b16-31d2a105be87"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9484f77a-3b17-66dd-4cff-27dc974056f4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("94a7e783-5867-8643-5841-407db77890d6"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9504817a-86de-bed4-2de7-6f616fbd4228"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("95c47825-c22f-0ce0-4aa1-0a033ad2313e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("984786af-f85a-f610-4ebf-060be174f81d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("99e17c62-3b9c-21c6-8728-30539859c579"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9a64a5fd-4f33-6220-0540-891969df66f9"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9c686426-799e-3cd1-8757-357bd6a166a4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a00ac0d4-3c12-18c7-a439-4c55dd980bc1"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a7abcf02-20b6-d4db-2502-12c06dfdf07b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a89c08ba-65e3-56a5-60f5-dbafb33e4691"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("aab06f51-5fbd-627f-828b-da4e0ae8ec5f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("afe553ed-32d2-7fee-1a44-e315c3ae8c90"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b0a3677e-d2e3-d465-7341-196900a74d41"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b1467ee5-a030-53e4-15e2-98dbcfa77342"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b7c4135f-dcc0-e164-6f7e-11956b1a2419"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("bc715a56-7ffc-a696-2437-a491be31a79e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c2227fbb-9e87-b2e1-b8b3-a6523674aac0"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c7d07363-9239-0c0c-40b3-45080ca4561b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c8450ca1-8111-3521-f02c-798ba267e769"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("cbd24427-9e0f-5b5c-a36a-3e7383f9b60c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("cc51572b-4ee4-634b-c55b-3ba15f072712"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ce58e1c0-39d7-7a23-b9fc-a8a1496752b8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d07e1f81-0d42-3083-7b94-011d6b59afaa"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d0aa4331-fdbd-f70e-7da0-ccb47d996683"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d0ed2fca-b16e-972d-79ab-5c89e6f9cbab"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d2b6bf45-9952-75a1-8571-2413a757d2f8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d50e48fb-26d1-5a9e-6677-569260a744bf"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d5d8ca91-72f8-b942-6bd9-b363c19886e4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d79169b8-f04a-de8d-b722-28586525e447"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d9a9eb7b-c062-b903-3aff-1b414fe8906c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("da3f1f0a-d2dd-9d7d-23d0-6ff7f84ee4cd"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("daced8e7-d00a-55e1-1174-f3786b1b5e4d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("dad0608e-ebbf-4967-05c4-5243761fc7c5"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("dba1e3b0-591b-6baa-1a27-676f1672e7b8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("dbdc8059-5ff4-992d-e6ed-6f1d86d1c736"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("de21cb6b-8375-2700-e512-ae572b02ba48"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("dfba8f92-b95b-24af-b050-0e386c4a4349"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e590888e-204d-5e58-84c8-46b73ae9f273"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e5feb548-adfb-2d7c-8caf-88e13dc17290"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e6c0db40-1e4b-574a-59ed-996022549221"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e97b8880-d045-33a4-3685-1c5cbac1beb7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ea70c14a-b72d-918e-81f1-aa5566eb1e17"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("eaefbf94-a67d-9d33-cbf5-cd4011b9917a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ec2d156e-3450-c12f-47fc-44be525497dc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("eeff9151-035e-114c-a4b8-b47230a390ed"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("efb5fb9e-3a68-978c-7dde-2a305c369a36"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f24eafe3-5b93-cbb6-6dcc-f2165548a2a7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f52ff85e-dabb-6c0c-5d51-8f0404c13381"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f56c914d-2562-1ad4-8462-172ce4ca25ad"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f5b9c24e-359e-de41-e0e9-9332c38cde13"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f5deba38-7e3b-ac52-9142-d86c4c8870cc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f5fef0f3-daab-7574-d322-6b593e1b037a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f99cdf49-7d17-f511-4405-e77d01a159cc"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fc606c9f-5d25-0602-192c-35e0176198b1"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fd4a27e0-4455-94c0-5a03-bc0f02ec094b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fe54988e-fb68-28b0-4529-033b52c77654"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ff92890f-dd88-8a76-e6f5-81ca324651c0"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0761f0d3-9538-cb1a-0567-2a2688758f77"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("08877fab-aa11-4280-5064-4ea65ce68431"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f092-7180-70af-de66-08f4dbc38b44"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0df582d4-5cfb-142b-b463-c2a9c99f77b9"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1823c892-3611-380b-80d0-0441fa9616af"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("33df7c88-aba8-a5d6-15dd-ecab9ec7d8f5"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4147b7ba-d58b-9347-f599-20e4a26a1459"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("46baf750-afaa-a6df-25f8-be25c4030396"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("47128444-22c0-6c9c-a6c6-08e9092d61bb"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("81b4863b-3305-e700-519f-7b935733ab79"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("92a7b352-6586-497c-9dd7-dfd96ab09c4f"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("94417086-b2e1-926e-f906-10354c29eed4"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("981d38df-be64-d403-71f2-931cdbd8145e"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9b6afe20-efae-b65a-f874-9a204d0934c0"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27e640d-0e2a-593d-076c-79e03a865aa6"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b70a712e-865b-04bf-a392-cb31612c6602"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cd23ae92-016f-a392-71df-6bcabeb13658"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d243f093-453e-c659-371e-cbfea2afe06f"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("dcd398d4-d765-62e7-8fc8-72c86f8e28ea"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e09f929e-6f3c-722a-87f3-4ac140cbc805"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ef96842e-8c24-101f-8659-f94681f081eb"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f8232f6b-91ca-e0e3-c107-a7f160498688"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f93efc9c-07c6-acbc-49de-3b76c3114274"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("20bededb-1c1b-db05-f84c-9259d2b7576e"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("652a4793-984d-cf69-18d2-336dda12226c"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417"));
        }
    }
}
