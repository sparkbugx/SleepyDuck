using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AcronymsAndSharperAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0c9b4df9-ce9f-cc8b-c914-c0e7e1d9c27c"),
                column: "Explanation",
                value: "A DLP (data loss prevention) block shows a policy tip about sensitive content, and it would affect adatum.example as well.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("10cc3734-0031-aa3e-e984-75315db1327f"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "Policy is processed on the client. Refreshing a DC (domain controller)'s own policy does nothing for a sales PC.", "Run gpupdate /force on the domain controller that holds the PDC (primary domain controller) emulator role, and then wait 90 minutes" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("132e813f-8b42-bd31-f944-ba2404333338"),
                column: "Text",
                value: "The DNS (Domain Name System) Server log on the nearest domain controller");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("17bfefb0-2c76-8c72-58d0-99ef99e470da"),
                column: "Text",
                value: "Block the sending IP (Internet Protocol) address in the connection filter policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1ef30f6d-e145-2a62-9f7f-663f8beff3dd"),
                column: "Explanation",
                value: "CDRs (call detail records) show that the call connected and how long it lasted. They don't show which way the media was flowing.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("20b7f8ce-6954-70cd-c02f-e2e4f8330e78"),
                column: "Explanation",
                value: "ZAP (zero-hour auto purge) only acts when a verdict changes, and you can't count on its timing while users are clicking.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("28d290c1-67d7-3438-b582-c4cb284ed62c"),
                column: "Text",
                value: "Share permissions override NTFS (New Technology File System) permissions");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2d5856ff-6304-b4a3-617f-e89b79261b65"),
                column: "Explanation",
                value: "The ARP (Address Resolution Protocol) table shows local IP-to-MAC mappings and tells you nothing beyond the LAN.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("37565c03-96de-a4bc-caac-d9e509142ab9"),
                column: "Text",
                value: "A completed MFA (multi-factor authentication) registration");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3a3d9577-8936-8ded-7251-0257cd724c08"),
                column: "Text",
                value: "Whether the disclaimer text is over a length limit, or contains HTML (Hypertext Markup Language) that the rule can't insert into the vendor's messages");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("40cce026-bde9-b83d-6a2d-88d6a040921e"),
                column: "Text",
                value: "DNS (Domain Name System) caching delayed the new certificate from reaching the Macs");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("426294d2-2f48-c7fd-af81-77baa64d0e6b"),
                column: "Text",
                value: "Fabrikam's own tenant has a cross-tenant access policy that blocks B2B (business-to-business) collaboration with outside organizations");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4837be8e-c4f9-ce45-cec0-3d498221ef82"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "802.1X happens before an IP (Internet Protocol) address is assigned, and these drops are authentication failures.", "Check the DHCP (Dynamic Host Configuration Protocol) lease for each Mac on the staff VLAN (virtual local area network)" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("489c3817-f7c6-0a14-808b-e00816f438af"),
                column: "Text",
                value: "Ask the DNS team to lower the record's TTL (time to live) and wait for the old answer to age out of the cache");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4e8947af-7831-58aa-1bc3-2f09b7817d4d"),
                column: "Text",
                value: "Ping the domain controller's IP (Internet Protocol) address");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5f2f4070-04f6-deb2-80b5-f4c6633b7e51"),
                column: "Text",
                value: "The gateway's ARP (Address Resolution Protocol) cache is full and is blocking new devices");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6ec397f0-5958-26ec-3d74-3673c533c591"),
                column: "Text",
                value: "Check the RTP (Real-time Transport Protocol) packet statistics for the failed call on the PBX to see where audio stopped flowing");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7250e01a-749d-f73d-2146-d0bc98c9fae2"),
                column: "Text",
                value: "The jitter buffer on the phones is too small for this WAN (wide area network) link");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7afbc17a-757b-70c9-1e15-6decf95f63f5"),
                column: "Text",
                value: "Check the vendor's SPF (Sender Policy Framework) record");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("81d30037-002b-6cf2-806f-a882ffb8fa80"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "DNS resolves names and has no part in assigning a client's IP (Internet Protocol) address.", "A DNS (Domain Name System) server has handed out an address from the wrong zone" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8efd53bc-b6e1-67a8-001d-2e9d1a9c2043"),
                column: "Text",
                value: "The destination is hiding its true loss by blocking ICMP (Internet Control Message Protocol)");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9504817a-86de-bed4-2de7-6f616fbd4228"),
                column: "Text",
                value: "The DNS (Domain Name System) Server log on a domain controller");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("99e42082-f570-da27-e005-d0f238a95d15"),
                column: "Text",
                value: "Open TCP (Transmission Control Protocol) port 5060 inbound from the hosted PBX to the phones so their audio can reach the branch");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a2cc52b8-32bc-fb47-8faa-f9b6be9c06ed"),
                column: "Text",
                value: "The domain's SPF (Sender Policy Framework) record is failing");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a46b90b1-cb89-d9e0-c063-acd68f8f905b"),
                column: "Text",
                value: "Assign static IP (Internet Protocol) addresses to each device that can't connect");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a4c552d0-a928-8e53-1f0a-444167405c29"),
                column: "Text",
                value: "Publish a DMARC (Domain-based Message Authentication, Reporting and Conformance) policy of p=reject for your own domain");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("aab06f51-5fbd-627f-828b-da4e0ae8ec5f"),
                column: "Text",
                value: "The WMI (Windows Management Instrumentation) filter on the Default Domain Policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b0a3677e-d2e3-d465-7341-196900a74d41"),
                column: "Text",
                value: "NTFS (New Technology File System) permissions are skipped for network access, and they only apply to people signed in at the server itself");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b6326f9b-e01d-bd56-b5fd-6ef99ede8435"),
                column: "Text",
                value: "The PBX (private branch exchange) searched its dial plan, found no such extension, and rejected the call with a timeout");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c491f14c-8826-4811-4196-b996ec9498bd"),
                column: "Text",
                value: "SIP (Session Initiation Protocol) registration is failing intermittently at the branch");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c8450ca1-8111-3521-f02c-798ba267e769"),
                column: "Text",
                value: "Take ownership of the folder from TrustedInstaller, then turn on NTFS (New Technology File System) compression for it from the Properties page");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c858e2cb-1469-9c66-afb5-356c113f6c6c"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "Missing QoS (quality of service) markings lower priority under congestion. They don't remove one direction of audio completely.", "DSCP (Differentiated Services Code Point) markings are missing from the voice traffic, so the ISP (internet service provider) is dropping the return RTP packets" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d25780cf-6fae-db32-e489-dcc369b881c8"),
                column: "Text",
                value: "Raise the WAN interface MTU (maximum transmission unit) to 9000 to improve throughput");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e3cdfc1b-fcfc-413e-38d5-35e46137a36e"),
                column: "Text",
                value: "The default gateway is performing NAT (Network Address Translation) for the guest VLAN");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e40a12c1-8d60-2324-7be9-d0af906cb580"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "Ping proves basic reachability with ICMP (Internet Control Message Protocol). It says nothing about which addresses and ports RTP is using.", "Run a sustained ping from a phone's VLAN (virtual local area network) to the hosted PBX address" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ebea3b74-0c06-f68b-f8f2-e769ed88a873"),
                column: "Text",
                value: "The client was given a reserved address by the DHCP server because its MAC (media access control) is in the exclusion list");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("eda27977-a3ce-a622-4683-d0b6c34ead2a"),
                column: "Text",
                value: "The caller failed digest authentication on the PBX (private branch exchange)");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("efb5fb9e-3a68-978c-7dde-2a305c369a36"),
                column: "Text",
                value: "The VM (virtual machine) had been off for more than 30 days, so Active Directory expired its machine account password");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f52ff85e-dabb-6c0c-5d51-8f0404c13381"),
                column: "Text",
                value: "The revert gave the VM (virtual machine) a different IP address");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fbad0072-7ed7-c0b0-b4b4-3f0a2033a9be"),
                column: "Text",
                value: "The SRTP (Secure Real-time Transport Protocol) keys don't match, so the phone is discarding the audio");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3"),
                column: "TechnicalExplanation",
                value: "A full system drive stops updates, logging, and sometimes the applications themselves, so act quickly, but measure before deleting anything. List the largest folders first. On servers the space usually goes to logs that nobody rotates, such as IIS (Internet Information Services) logs under C:\\inetpub\\logs\\LogFiles, along with dump files, temp folders and old installers. The WinSxS folder is the Windows component store. It holds the files Windows needs to service itself, and Microsoft warns that deleting files from it can leave a system unable to boot or update. Reduce it with the built-in tools. Dism /Online /Cleanup-Image /AnalyzeComponentStore reports how much can be reclaimed, and /StartComponentCleanup removes superseded component versions straight away, where the scheduled task waits 30 days. Adding /ResetBase removes every superseded version, and after that the updates already installed can't be uninstalled. Then fix the cause: put retention on the logs or move them to a data volume, and alert on free space well before it's critical.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Every desk phone at a branch office can place and answer calls through the company's hosted PBX (private branch exchange), but on all calls the staff hear nothing while the people they call hear them clearly. Phones show as registered, and the problem began after the branch firewall was replaced.", "SIP (Session Initiation Protocol) sets up a call, and RTP (Real-time Transport Protocol) carries the audio as a separate UDP (User Datagram Protocol) stream, so a call can connect perfectly with audio failing in one direction. Each side uses the SDP (Session Description Protocol) body in the SIP messages to announce the IP (Internet Protocol) address and port where it wants to receive RTP. When a phone behind NAT (Network Address Translation) advertises its private address, the far end sends audio to an address it can't route to. This is the classic one-way audio pattern, and it fits here because the new firewall changed how NAT was handled. Fix it with proper NAT traversal, such as STUN (Session Traversal Utilities for NAT), or an SBC (session border controller) or PBX (private branch exchange) that uses symmetric RTP. Also disable SIP ALG (Application Layer Gateway) if the firewall's rewriting is making things worse, and make sure the RTP UDP port range is allowed. Compare captures from both sides of the firewall, because they show whether a stream was never sent or was lost on the way." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76"),
                column: "TechnicalExplanation",
                value: "A SIP (Session Initiation Protocol) 408 means a request got no final response within the allowed time. For an INVITE over UDP (User Datagram Protocol), Timer B expires at 64 times T1, which is 32 seconds with the default 500 ms T1, and that matches this ticket. Retransmissions with no provisional response show the INVITE isn't reaching the phone, or the phone's reply isn't getting back. Outbound calls work because the phone opens its own path through the router. Inbound calls depend on a NAT (Network Address Translation) or firewall UDP mapping staying open. When the mapping expires between registrations, the PBX (private branch exchange)'s registration is still valid, but its INVITE is dropped at the router. Keep-alives sent more often than the router's UDP timeout hold the mapping open. Avoid changing SIP timers, because a longer wait just delays the same failure.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("342cc552-23d1-4282-8205-22cb79b47a8e"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "At 10:40, three users report an email titled 'Payroll update required' that links to a fake Microsoft sign-in page. It came from hr-notice@payroll-portal.example and passed SPF (Sender Policy Framework) and DKIM (DomainKeys Identified Mail) for that domain. The tenant has Microsoft Defender for Office 365 Plan 2.", "Threat Explorer in Defender for Office 365 Plan 2 finds every copy of a campaign by sender, subject or URL (uniform resource locator), and shows each copy's delivery location along with URL clicks recorded by Safe Links. You can act on the results directly, for example with a soft delete, and the Action center tracks the remediation. Zero-hour auto purge (ZAP) can also remove delivered messages when a verdict changes, but you can't rely on its timing during an active campaign. A sender that passes SPF (Sender Policy Framework) and DKIM (DomainKeys Identified Mail) for its own lookalike domain isn't spoofing you, so your own DMARC (Domain-based Message Authentication, Reporting and Conformance) policy won't stop it, and the Tenant Allow/Block List is the place to block the domain and the URL. Anyone who clicked through to the credential page may have given away a password, so reset it, revoke their sessions and review their sign-ins." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e"),
                column: "TechnicalExplanation",
                value: "A shared folder has two sets of permissions. Share permissions apply only to access over the network, and NTFS (New Technology File System) permissions apply to every kind of access. Over the network both are evaluated and the more restrictive result wins, so Full Control on the share with Read in NTFS gives Read. Common practice is to keep share permissions broad, for example Authenticated Users with Full Control or Change, and to do the real control in NTFS with groups. Within one set, a user's permissions from all their groups add up, but a Deny entry beats any Allow. The Effective Access tab under Advanced Security Settings calculates the NTFS result for a named user. Group membership has its own catch. A user's access token, which lists their groups, is built when they sign in. Adding someone to a group changes Active Directory, but not a token that already exists, so the user has to sign out and back in. whoami /groups shows the groups in the current token. Grant access to groups and not to individual accounts, or the permissions become unmanageable.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb"),
                column: "TechnicalExplanation",
                value: "Domain controllers copy changes to each other, and when that stops, each site slowly works from a different version of the directory. Start with repadmin /replsummary for a forest-wide view of failures and how old they are, then repadmin /showrepl on the failing DC (domain controller) for each partition's last attempt, result, and last success. Error 1722, 'The RPC (remote procedure call) server is unavailable', is a connectivity error: the destination DC couldn't connect to the source DC. Microsoft notes that DNS (Domain Name System) lookup failures cause a large share of these, because DCs locate each other through records in the _msdcs zone. dcdiag /test:dns checks that, and nltest /dsgetdc tests the DC locator. A DC must use internal DNS servers that host the Active Directory zones. A public resolver doesn't hold those records, and an alternate server is only tried when the preferred one doesn't answer. Other causes of 1722 are blocked ports, namely TCP (Transmission Control Protocol) 135 for the endpoint mapper and the dynamic RPC range. Don't leave replication broken for long. A DC that stays out of sync past the tombstone lifetime, usually 180 days, can bring deleted objects back.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("652a4793-984d-cf69-18d2-336dda12226c"),
                column: "TechnicalExplanation",
                value: "An account locks when the count of bad passwords passes the lockout threshold, so a lockout that repeats on a timer means something is still sending the old password. A domain controller that sees a bad password checks it with the PDC (primary domain controller) emulator, which always has the newest password, and lockouts are processed there. That makes the PDC emulator's Security log the place to look for event 4740, 'A user account was locked out'. Find that DC (domain controller) with (Get-ADDomain).PDCEmulator. The event's Caller Computer Name field names the computer the failed attempt came from. On that computer, event 4625 records each failed logon with its logon type: 2 is interactive, 3 is network, 4 is batch (scheduled tasks), 5 is service, and 10 is Remote Desktop. Usual culprits are scheduled tasks, services, mapped drives, saved entries in Credential Manager, disconnected Remote Desktop sessions, and mail apps on phones. Event 4740 is only logged when Audit User Account Management is enabled.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Users at a branch office report choppy video calls and stalls in a cloud app at 203.0.113.50, starting this morning. Everything on the local file server works normally. You're on a Linux machine on the branch LAN (local area network).", "Ping is the quickest way to scope a problem: test the default gateway first, then an external target. MTR (My Traceroute) then combines ping and traceroute by sending a stream of probes to every hop. The rule for reading it is that loss only matters if it continues through to the final destination. A router in the middle showing loss when the hops after it don't is usually just rate limiting the ICMP (Internet Control Message Protocol) or TTL-expired replies it generates, and it's forwarding traffic fine. Real loss starts at one hop and carries through to the end, which points to that hop or the link leading to it. Paths are often asymmetric, so run MTR from the far side too if you can, and check interface errors and utilization on your own equipment before escalating. Give the ISP (internet service provider) timestamped MTR reports from both directions." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("6f67ae98-4a03-2943-e2d3-cd7ab58e559a"),
                column: "TechnicalExplanation",
                value: "NDR (non-delivery report) 5.7.520 means 'Access denied, your organization does not allow external forwarding'. Automatic external forwarding is controlled by the outbound spam filter policy, whose default setting (Automatic - System-controlled) behaves as Off. Attackers often use forwarding rules to exfiltrate mail from compromised mailboxes, so leave the Default policy alone and enable forwarding only through a custom policy scoped to the people with an approved business need. Check any remote domain settings as well, because they can also stop automatic forwarding.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "A colleague built a GPO (Group Policy Object) called Sales Drive Map with user settings and linked it to the Staff OU (organizational unit). To limit it to the sales team, they removed Authenticated Users from Security Filtering and added the Sales-Users group. Nobody in Sales gets the drive, and gpupdate /force doesn't help.", "A GPO (Group Policy Object) applies to a user only if it is linked where the user's account lives, and the user has both Read and Apply group policy permissions on it. Security Filtering is a shortcut for granting those two permissions. There's one more requirement that catches people out. Since security update MS16-072 in June 2016, Windows fetches user policy with the computer's security context, where it used to use the user's. The computer account therefore needs Read permission on any GPO that applies to the signed-in user. By default it gets that through Authenticated Users, a group that includes computer accounts. Removing Authenticated Users from Security Filtering takes away Read as well as Apply. The fix is on the GPO's Delegation tab: add Authenticated Users or Domain Computers with Read only. That lets computers read the GPO without applying it to everyone. On the client, gpresult /h produces a report of which GPOs were applied or denied, and why. A Deny entry always beats an Allow." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("95e42524-b675-8aab-3bae-f938012f09ed"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Your team moved intranet.corp.example to a new server this morning and updated DNS (Domain Name System). Windows laptops reach the new site, but a MacBook still loads the old server. The Mac has a normal internet connection, and browsing elsewhere works fine.", "macOS caches DNS (Domain Name System) results in the mDNSResponder daemon, and most apps resolve names through that system resolver. Tools such as dig and nslookup query DNS servers directly and skip the system resolver, while dscacheutil -q host follows the path that apps use. That makes comparing them a good test: if dig shows the new address and dscacheutil shows the old one, the fault is on the Mac rather than in DNS. Clear it with sudo dscacheutil -flushcache followed by sudo killall -HUP mDNSResponder. The HUP (hang-up) signal tells the daemon to drop its cache without stopping it. If the old answer survives a flush, look at what the system resolver checks besides DNS, such as an entry in /etc/hosts, or a scoped resolver from a VPN (virtual private network), which scutil --dns will list." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24"),
                column: "TechnicalExplanation",
                value: "This error means the client never reached a working RDP (Remote Desktop Protocol) listener, and Microsoft's guidance splits the causes into the listener and the network. With console access, run qwinsta on the server. A line for rdp-tcp in the Listen state means the listener is up. If it's missing, check that fDenyTSConnections is 0, both under HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server and in the policy path if one is set, and that the Remote Desktop Services service (TermService) is running. Next, connect from the console with mstsc to localhost. If that works, the server side is healthy and the fault is between client and server. Test-NetConnection with -Port 3389 from another machine tests the TCP (Transmission Control Protocol) port itself, which ping can't do because ping uses ICMP (Internet Control Message Protocol). A False result points to the Windows Firewall rules named Remote Desktop - User Mode (TCP-In and UDP-In), a network firewall, or a changed port number. Failures after that point, such as credentials, Network Level Authentication, certificates, or membership of Remote Desktop Users, produce different messages because the connection has already been made.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750"),
                column: "TechnicalExplanation",
                value: "SharePoint and OneDrive sharing works in layers. The organization-level setting in the SharePoint admin center is the most permissive level allowed, and each site can match it or be more restrictive. Domain restrictions apply on top of both, either for the whole organization under Policies > Sharing ('Limit external sharing by domain') or for a single site. Sharing with a domain that isn't on an allow list fails with this policy message, which is why one partner works and another doesn't. SharePoint also uses Microsoft Entra B2B (business-to-business) for guests, so an allow or deny list in the Entra external collaboration settings can block the invitation too, and the two lists should agree. The narrow fix is adding the approved domain to the allow list, not loosening the sharing level or removing the restriction for everyone.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "MacBooks on the WPA2-Enterprise (Wi-Fi Protected Access 2, Enterprise mode) SSID (service set identifier) CorpSecure started dropping off Wi-Fi about 8 hours after connecting, and they fail to rejoin. It started the day after the RADIUS (Remote Authentication Dial-In User Service) server's certificate was renewed. Windows laptops are fine. The Macs connect using a configuration profile from your MDM (mobile device management).", "802.1X authenticates a device before it's given network access. With PEAP (Protected Extensible Authentication Protocol) or EAP-TLS (Extensible Authentication Protocol with Transport Layer Security), the client validates the RADIUS (Remote Authentication Dial-In User Service) server's certificate during the TLS handshake. On macOS, the eapolclient process handles this, and an MDM (mobile device management) profile normally defines which certificates and server names to trust. Macs stay connected on an existing session, then fail when reauthentication happens, which explains drops long after the certificate change. Here the 8-hour pattern matches the session timeout. Windows machines were fine because they trust the new chain another way. The right fix is to push an updated profile containing the new CA (certificate authority) chain and trusted server names. Telling users to click Trust or turning off validation trains people to accept a fake access point, which is how credentials get stolen. On Windows NPS (Network Policy Server), event 6272 means access was granted and 6273 means it was denied." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417"),
                column: "TechnicalExplanation",
                value: "Every domain-joined computer has its own account in Active Directory, with a password that the computer and the domain both know. The computer uses it to build a secure channel to a domain controller, and domain sign-ins on that computer depend on the channel. By default the computer itself starts a password change every 30 days. Active Directory never expires a machine password, so a PC that was simply switched off for months still works. The usual real causes are restoring an old checkpoint or image, which brings back a password the domain has since replaced, or a computer account that was reset, deleted, or taken over by another machine with the same name. Test-ComputerSecureChannel reports whether the channel is healthy, and adding -Repair with a domain credential resets it in place. Reset-ComputerMachinePassword is the next option. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort. Don't delete the computer object, since a new one has a different SID (security identifier) and loses its group memberships.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Luis from Sales is at a client site in Lisbon and can't sign in to Outlook or Teams. The browser shows error AADSTS53003, 'Access has been blocked by Conditional Access policies'. No MFA (multi-factor authentication) prompt appears, and the same account worked from the office yesterday.", "Microsoft Entra ID evaluates every sign-in against the Conditional Access policies that apply to it, and the sign-in log records the result of each policy. AADSTS53003 means a policy's grant control was Block, which is why no MFA (multi-factor authentication) prompt appeared: a block is applied before any challenge is offered. Country-based named locations are resolved from the sign-in's public IP (Internet Protocol) address, so a user abroad falls outside an 'allowed countries' location. The supported way to make an exception is an exclusion group with a documented end date, ideally governed by an access review. Disabling the policy, switching it to Report-only, or trusting a hotel IP weakens protection for everyone. The What If tool evaluates a simulated sign-in against every policy, so you can prove the exception works before the user tries again." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "At 9 AM, new devices on the staff and guest VLAN (virtual local area network) 20 (192.168.20.0/24) can't get online and show 169.254.x.x addresses. Devices that were already connected still work. The VLAN uses a Windows DHCP (Dynamic Host Configuration Protocol) server, and a conference has been running at the office all week.", "A 169.254.x.x address is an APIPA (Automatic Private IP Addressing) link-local address, which a client assigns itself when no DHCP (Dynamic Host Configuration Protocol) server offers a lease. Devices that already hold a lease keep working, which is the classic sign of exhaustion rather than a server outage. A /24 has only 254 usable addresses, and a Windows DHCP scope defaults to an 8-day lease, so visitors' phones keep addresses long after leaving. Phones using private (randomized) MAC (media access control) addresses can make this worse, since one device can look like several clients. Shorten the lease on busy guest networks, and reclaim stale leases. Changing the duration only affects new leases and renewals, so it needs some time or lease cleanup. Turn on conflict detection while reclaiming, since a device might still be using a removed address. Then fix capacity properly by widening the subnet or moving guests to their own VLAN (virtual local area network)." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03a808f5-e30b-8239-33fc-4b53b61f7f8c"),
                column: "Explanation",
                value: "Clicking past the warning means the user reached the fake sign-in page and may have typed a password. Assume they did. Reset the password, revoke sessions so stolen tokens stop working, and review the sign-in logs for sign-ins from unfamiliar locations. Also check for new inbox rules, forwarding and newly registered MFA (multi-factor authentication) methods, which attackers add to keep access.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("03fe44d1-045b-dd43-5c2a-8f28a9bf0e88"),
                column: "Prompt",
                value: "Where do you grant it in the admin UI (user interface)?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"),
                column: "Prompt",
                value: "dig returns the new IP (Internet Protocol), and dscacheutil returns the old one. What does that show?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"),
                column: "CorrectAction",
                value: "The policy blocked the sign-in because Portugal isn't in the allowed countries named location");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"),
                column: "Explanation",
                value: "The permissions are right. The share lets any authenticated user through with Full Control, and NTFS gives Modify to Finance-RW, so NTFS decides and members get Modify. Tomas is a member in Active Directory, but whoami /groups shows his current access token, and the group isn't in it. Windows builds the token once, at sign-in, from the groups he had at that moment. He signed in this morning and was added an hour ago. The token carries the SID (security identifier) of every group he belongs to, and the file server compares those SIDs with the entries in the folder's ACL (access control list). His token has no SID for Finance-RW, so no entry grants him access and the server denies it.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"),
                column: "Prompt",
                value: "You find 38 GB of IIS (Internet Information Services) logs going back four years in C:\\inetpub\\logs\\LogFiles, and a 14 GB WinSxS folder. What is the best first move?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"),
                column: "Explanation",
                value: "Over UDP (User Datagram Protocol), SIP resends a request that gets no response. Timer A starts at T1, which is 500 ms by default, and doubles each time, which gives the gaps of 0.5, 1, 2 and 4 seconds. A phone that receives an INVITE answers almost at once with 100 Trying, then 180 Ringing. Neither appears, so the INVITE never arrived, or the answers were lost. After 32 seconds, which is Timer B, the PBX gives up and returns 408 to the caller.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3af3551d-598b-03d4-073b-0476f20f2ae5"),
                column: "Explanation",
                value: "Repeat the tests that failed. Test-NetConnection on port 3389 should return True, and an administrator should be able to open a session from their own PC. Then find what disabled the rule. If the hardening script or GPO (Group Policy Object) still does it, the rule goes off again at the next run or policy refresh. Fix it at the source, with an exception that allows RDP from the admin network.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("3b41f970-ac1e-417f-a972-41a2dc3fbc75"),
                column: "Explanation",
                value: "Every sign-in attempt is logged in Entra ID under Sign-in logs, including failed ones. The failed entry gives the error code, the IP (Internet Protocol) address and the location it mapped to, the app, and the device. Its Conditional Access tab lists every policy that was evaluated and whether each one succeeded, failed or didn't apply. That names the policy that blocked Luis, so you don't have to guess.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"),
                column: "CorrectAction",
                value: "Confirm the profile is installed, then verify NPS (Network Policy Server) event 6272 through a full session timeout");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("46baf750-afaa-a6df-25f8-be25c4030396"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "BR-DC02 can't connect to the RPC (remote procedure call) service on its source DC, so suspect DNS (Domain Name System) or the network path", "Replication is pull-based. BR-DC02 connects to HQ-DC01 over RPC and asks for changes. Error 1722, 'The RPC server is unavailable', means that connection couldn't be made. The causes are a name that doesn't resolve, a blocked port, which is TCP (Transmission Control Protocol) 135 for the endpoint mapper and then a dynamic port from 49152 to 65535, or a source server that is down. HQ-DC01 is serving headquarters, so look at DNS and the path between the sites." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"),
                column: "Explanation",
                value: "802.1X has three parts. The Mac is the supplicant, the access point is the authenticator, and the RADIUS server decides. To see why it fails you need both ends. On the Mac, the eapolclient process writes the EAP (Extensible Authentication Protocol) conversation to the system log, which you can read with the log show command. The RADIUS server's log gives its reason for each accept or reject. Match the two at the times of the drops.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Ignore hop 5, since loss that doesn't carry on is likely ICMP (Internet Control Message Protocol) rate limiting", "Each line of MTR is that router's reply to probes addressed to it, and routers generate those ICMP replies in their control plane, at low priority and often rate limited. Traffic passing through a router is forwarded by the data plane, which is separate and much faster. If hop 5 were dropping forwarded packets, hops 6 to 9 would show loss as well, because their probes pass through it. They show 0%, so hop 5 is forwarding fine. The rule is that loss only counts when it continues to the final hop." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5b1a587e-a9b6-bb9a-db52-8b4bb1ea699c"),
                column: "Explanation",
                value: "A domain controller finds its partner through DNS, using a CNAME (canonical name) record for the partner's GUID (globally unique identifier) in the _msdcs zone and then its host record. dcdiag /test:dns checks that the DC's DNS client settings are sound and that these records can be resolved and registered. DNS is the most common cause of 1722 and quick to test, so do it before looking at firewalls.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5cd1b589-61da-ec0e-197d-1d25db78d540"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Share permissions first, then NTFS (New Technology File System) permissions, and he gets only what both sets allow", "A network connection to \\\\FILE-SRV01\\Finance passes two gates. First the SMB (Server Message Block) server checks the share permissions on the Finance share, which only apply to access over the network. Then it checks the NTFS permissions on the folder itself, which apply to every kind of access. Windows works out his total in each set from all his groups, and the more restrictive of the two totals is what he gets. Full Control on the share with Read in NTFS gives Read, and Read on the share with Modify in NTFS also gives Read. For this ticket it means a block in either set is enough to cause 'Access is denied', so you have to look at both." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"),
                column: "Explanation",
                value: "repadmin /replsummary contacts every domain controller and prints one line each, as a source and as a destination: how many replication attempts failed, the longest time since a success, and the error code. It shows in seconds which DC (domain controller) is behind and for how long. repadmin /showrepl on that DC then gives the detail for each partition and partner.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"),
                column: "CorrectAction",
                value: "The mailbox is compromised, and the attacker's rule hides the bank's replies (business email compromise)");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"),
                columns: new[] { "CorrectAction", "Prompt" },
                values: new object[] { "Point BR-DC02 back at the internal AD (Active Directory) DNS servers, then clear its resolver cache", "The DNS test fails. BR-DC02's network adapter lists 8.8.8.8 as its preferred DNS server, set during an ISP (internet service provider) outage last week. What is the fix?" });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"),
                column: "Explanation",
                value: "In SDP, the c= line is the address where that side wants to receive audio. The phone put its private LAN (local area network) address there, 192.168.10.25. The provider sends its RTP to that address, and private addresses can't be routed on the internet, so those packets go nowhere. Audio from the phone still gets out, because outbound packets pass through NAT (Network Address Translation) normally. That's why the staff hear nothing and the other party hears them.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"),
                column: "CorrectAction",
                value: "Short-stay devices hold 8-day leases, exhausting the 254 addresses of the /24 scope");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "SIP (Session Initiation Protocol) signaling is working, so the fault is in the RTP (Real-time Transport Protocol) media path", "A VoIP (Voice over IP) call uses two protocols. SIP is the signaling that makes the phone ring and connects the call. RTP carries the audio, as a separate UDP (User Datagram Protocol) stream in each direction, on different ports. These calls ring and connect, so SIP works. One direction of audio is missing, so one RTP stream isn't arriving. That narrows the fault to the media path, and the new firewall is the obvious suspect." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("7159f258-9824-2105-ccae-acebca7cb3fc"),
                column: "Explanation",
                value: "Threat Explorer, under Email & collaboration > Explorer in the Defender portal, searches all mail in the tenant. Filter on the sender address, the subject or the URL (uniform resource locator), and it returns every copy with its recipient, where it was delivered (Inbox, Junk or quarantine), and whether anyone clicked the link. It comes with Defender for Office 365 Plan 2. Three reports usually mean many more recipients, and this is how you find the full scope.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"),
                column: "Prompt",
                value: "A later MTR shows 0% at hops 1 and 2, then 6% at hop 3, and about 6% on every hop through to the destination. Hop 2 is your firewall, and hop 3 is the ISP (internet service provider)'s gateway. What does this show?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72bf031d-2d13-1a34-549a-b71b4faffb68"),
                column: "Explanation",
                value: "Quarantined means Exchange Online Protection or Defender for Office 365 judged the message unsafe and held it, so it never reached any mailbox folder and the sender got no bounce. High confidence spam is the strongest spam verdict, an SCL (spam confidence level) of 9. In the Defender portal, go to Email & collaboration > Review > Quarantine. Check the sender, the authentication results and a preview before releasing, because an invoice is exactly what a phishing email imitates.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "User policy is fetched in the computer's security context, and no entry gives the PCs Read", "Since the MS16-072 update in 2016, Windows downloads user policy using the computer account, where it used to use the user's. So the computer needs Read on the GPO, both on its object in Active Directory and on its files in SYSVOL. Normally it has that through Authenticated Users, which includes computer accounts. Removing that group from Security Filtering took away Read as well as Apply. Nothing in this list covers the sales PCs, so they can't read the GPO, and it's skipped." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"),
                column: "Explanation",
                value: "The system resolver has sources other than DNS, and a flush doesn't touch them. The file /etc/hosts is read first, so a line for intranet.corp.example there, perhaps left from testing, overrides DNS permanently. scutil --dns lists every resolver the Mac uses, including ones a VPN (virtual private network) adds for specific domains, which may point at a DNS server that still has the old record.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("941e7c17-066e-1729-ea54-2d58af65c26d"),
                column: "Explanation",
                value: "Active users > Add a user creates the Entra ID account and walks through the basics, then the usage location and license, then optional roles and profile details, in one pass. The mailbox and other services are provisioned once the license is assigned. For many users at once you'd use a CSV (comma-separated values) import or PowerShell, but for one new hire the wizard is the standard route.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("981d38df-be64-d403-71f2-931cdbd8145e"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Have Tomas sign out and sign back in, so that Windows builds a new access token", "A new sign-in makes Windows build a new access token and request new Kerberos tickets, and the group SIDs inside them now include Finance-RW. Nothing on the server needs to change, because the permissions were already correct. Afterwards, whoami /groups should list Finance-RW, and the share should open. Running klist purge can refresh the tickets without a sign-out, but signing out and in is the reliable answer." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"),
                column: "Explanation",
                value: "A packet capture shows what's on the wire. In Wireshark, Telephony > VoIP Calls lists each call, and Flow Sequence draws the SIP messages. Inside the INVITE and the 200 OK is the SDP (Session Description Protocol), where each side states the IP (Internet Protocol) address and port it wants audio sent to. Telephony > RTP > RTP Streams shows which audio streams exist. With those you can see whether a stream was never sent or was sent to the wrong place.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9e9f93a6-1b29-36de-3739-271c1cd80551"),
                column: "Explanation",
                value: "Every domain-joined PC has a computer account in Active Directory with its own password, which the PC changes by itself every 30 days. The Netlogon service uses that password to open a secure channel to a domain controller, and it needs the channel to check users' sign-ins. If the password stored on the PC doesn't match the one in Active Directory, the domain controller refuses the channel. Every domain user on that PC then gets this error.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9f3c2bc3-09ba-ac1e-813f-517cf3b19e5d"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Sign in with a local administrator account, typed as .\\Administrator", "A local account is stored on the PC itself, in the SAM (Security Account Manager) database, and it's verified there without any domain controller. That's why it still works when the secure channel is down. Use the built-in Administrator or a local admin managed by LAPS (Local Administrator Password Solution), signing in as .\\Administrator so Windows checks the local database." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Ping the default gateway, then an external IP (Internet Protocol), each with about 100 packets", "Start by finding out which side of the gateway the loss is on. Ping the default gateway to test the LAN, then an external address to test the path beyond it. Use around 100 packets, for example ping -c 100, because the result is a percentage and a few packets can easily miss occasional loss. A clean gateway with loss outside points to the WAN (wide area network)." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a36f3e7c-c851-b3ce-5b5f-8261ccd91317"),
                column: "CorrectAction",
                value: "The usage location, a country or region set on the user account");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a77f3ca5-f1b0-8c8e-40fd-ae12f22d8932"),
                column: "Explanation",
                value: "A service that runs as a user account stores that account's password on the server, encrypted as an LSA (Local Security Authority) secret. At each start, Windows signs the account in with the stored password. The security team changed the password in Active Directory last week, and nobody updated the copy stored in the service. Event 7038 says exactly that: the currently configured password is incorrect.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a7ba522e-0217-6b71-33a8-76cb23de155d"),
                column: "CorrectAction",
                value: "Turn on 'Allow external senders to email this group' in the group's settings");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Trace the source of the bad password attempts before unlocking the account again", "Each wrong password raises the account's badPwdCount attribute, and the account locks when the count reaches the lockout threshold within the observation window set in the password policy. Dana isn't at her desk when it happens and it repeats every 20 minutes, so a device or program is sending her old password on a schedule. Unlocking resets the counter, and the next few attempts lock it again. The real job is to find the source, and the logs can name it." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"),
                column: "Explanation",
                value: "The suspect link has two ends, and one of them is yours. Check the firewall's WAN interface counters for CRC (cyclic redundancy check) errors, a speed or duplex mismatch, and utilization near the limit, since any of those would make it your fault to fix. Traffic often returns by a different route, and MTR only shows the outbound path, so a second MTR run from the far side toward you covers the return path.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b70a712e-865b-04bf-a392-cb31612c6602"),
                column: "Explanation",
                value: "A group managed service account, or gMSA, is an Active Directory account built for services. Active Directory generates and rotates its password automatically, every 30 days by default, and the servers allowed to use it retrieve the password themselves. Nobody types it, and no stored copy goes stale. It needs a KDS (Key Distribution Service) root key in the domain and a service that supports running as a gMSA.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "The client got no DHCP offer and assigned itself a link-local APIPA (Automatic Private IP Addressing) address", "A DHCP lease takes four messages: Discover, Offer, Request and Acknowledge. The client broadcasts a Discover and waits for a server's Offer. If no offer comes, Windows and macOS give themselves an address in 169.254.0.0/16, called APIPA or link-local. It only works on the local segment and has no gateway. So this address always means the client asked for a lease and got no answer." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"),
                column: "Explanation",
                value: "Test from the inside outwards. mstsc to localhost on the server uses the RDP service without crossing the network or the firewall, so if it works, the service is healthy. Then, from another PC, Test-NetConnection FILE-SRV03 -Port 3389 tries a TCP connection to the RDP port. TcpTestSucceeded: True means the port is reachable, and False means something is blocking it. Ping can't answer this, because it uses ICMP (Internet Control Message Protocol) and not TCP 3389.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"),
                column: "Explanation",
                value: "The far end needs a public address and port to send audio to. STUN (Session Traversal Utilities for NAT) lets the phone learn its public address and put it in the SDP. Or the provider's SBC (session border controller) can use symmetric RTP, where it ignores the SDP address and replies to wherever the phone's audio came from. The firewall must also allow the provider's RTP UDP port range. If SIP ALG (Application Layer Gateway) on the new firewall is rewriting packets badly, turn it off.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("caf329a9-cc48-c862-b83a-adbafdf11b1b"),
                column: "Explanation",
                value: "An NDR (non-delivery report), or bounce message, carries an enhanced status code. 5.x.x is a permanent failure, and 5.7.x is a security or policy block. 5.7.520 comes with the text 'Access denied, your organization does not allow external forwarding'. So Exchange Online in your own tenant refused the message before it left, and nothing at the partner is at fault. Reading the code first tells you which side to investigate.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cb4e9501-87f1-e8ad-655f-e8352e33230d"),
                column: "Explanation",
                value: "RDP (Remote Desktop Protocol) needs a listener on the server, which by default waits on TCP (Transmission Control Protocol) port 3389. qwinsta, short for query session, lists sessions and listeners. A line for rdp-tcp with the state Listen means the Remote Desktop service is up and accepting connections. If it's missing, the fault is on the server: RDP is disabled (fDenyTSConnections set to 1) or the TermService service is stopped. If it's there, the fault is between the client and the listener.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d243f093-453e-c659-371e-cbfea2afe06f"),
                column: "Explanation",
                value: "gpresult shows what Group Policy actually did on this PC for this user. The /h switch writes an HTML (Hypertext Markup Language) report that lists the applied GPOs, the denied GPOs with a reason such as security filtering or access denied, and the user's group memberships. Run it as the affected user. It replaces guessing in the management console with evidence from the client.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d416ab8d-e55a-d48e-bbc5-60b9c0393fce"),
                column: "Explanation",
                value: "WinSxS is the component store, which holds every version of Windows components so that updates can be installed and rolled back. It must only be cleaned with DISM (Deployment Image Servicing and Management). /AnalyzeComponentStore reports its real size and whether cleanup is recommended. /StartComponentCleanup then removes versions that newer updates have replaced. Deleting files in it by hand can leave Windows unable to update or boot.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"),
                column: "Explanation",
                value: "When the phone registers, the home router creates a NAT (Network Address Translation) mapping from the phone to the PBX, and the PBX sends calls back through that mapping. The router deletes an idle UDP mapping after 60 seconds. The registration stays valid for much longer, so the PBX keeps sending INVITEs to a mapping that no longer exists, and the router drops them. A keep-alive every 20 to 30 seconds keeps the mapping open. This explains the other clues too. A reboot re-registers and opens a new mapping for a minute, and outbound calls open their own.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"),
                column: "Explanation",
                value: "MTR (My Traceroute) combines traceroute and ping. It finds every router on the path and then probes each one many times, reporting loss and latency per hop. In this command, -r is report mode, -w keeps full hostnames, and -c 200 sends 200 probes to each hop. With 200 probes, a 5% loss shows up reliably.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"),
                columns: new[] { "Explanation", "Prompt" },
                values: new object[] { "SIP response codes follow the same pattern as HTTP (Hypertext Transfer Protocol). 4xx is a request failure, and 408 Request Timeout means a request was sent and no final response came back in time. It doesn't say why. A 408 is generated by whichever device gave up waiting, so the work is to find how far the INVITE got.", "What does SIP (Session Initiation Protocol) 408 Request Timeout tell you?" });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e7de2726-9058-4dab-6026-21eb4d335180"),
                column: "Explanation",
                value: "Connect-ExchangeOnline comes from the ExchangeOnlineManagement module, which you install once with Install-Module ExchangeOnlineManagement. It signs in with modern authentication, so MFA (multi-factor authentication) works, and it loads the Exchange cmdlets such as Get-InboxRule and Set-Mailbox. The older ways in are gone. Basic authentication for remote PowerShell is switched off, and the MSOnline module is retired.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e8673c4e-05bf-a7b6-0142-4ce806209b8a"),
                column: "CorrectAction",
                value: "The Security log on the PDC (primary domain controller) emulator, filtered to event 4740 for Dana's account");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"),
                column: "Explanation",
                value: "With -Repair, the cmdlet sets a new machine password and writes it to both the PC and its account in Active Directory, using the domain credential you supply. The computer object, its SID (security identifier) and its group memberships stay as they are, and normally no restart is needed. Reset-ComputerMachinePassword is the alternative. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"),
                columns: new[] { "Explanation", "Prompt" },
                values: new object[] { "In PEAP (Protected Extensible Authentication Protocol) and EAP-TLS (Extensible Authentication Protocol with Transport Layer Security), the RADIUS server presents its certificate first, and the client checks it before sending any credentials. The Mac's configuration profile lists exactly which CA and server names to trust. The renewed certificate comes from a chain that isn't in that list, so the Mac ends the handshake with an unknown CA alert. The password is never sent, so it can't be the problem.", "The Mac logs show a TLS (Transport Layer Security) failure during EAP, and RADIUS logs an unknown CA (certificate authority) alert from the client at each drop. Passwords are correct. What is the cause?" });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f430db22-4555-1b5b-092d-f0e293522887"),
                column: "CorrectAction",
                value: "Whether the label's container settings turn off external user access for the team");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0c9b4df9-ce9f-cc8b-c914-c0e7e1d9c27c"),
                column: "Explanation",
                value: "A DLP block shows a policy tip about sensitive content, and it would affect adatum.example as well.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("10cc3734-0031-aa3e-e984-75315db1327f"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "Policy is processed on the client. Refreshing a DC's own policy does nothing for a sales PC.", "Run gpupdate /force on the domain controller that holds the PDC emulator role, and then wait 90 minutes" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("132e813f-8b42-bd31-f944-ba2404333338"),
                column: "Text",
                value: "The DNS Server log on the nearest domain controller");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("17bfefb0-2c76-8c72-58d0-99ef99e470da"),
                column: "Text",
                value: "Block the sending IP address in the connection filter policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1ef30f6d-e145-2a62-9f7f-663f8beff3dd"),
                column: "Explanation",
                value: "CDRs show that the call connected and how long it lasted. They don't show which way the media was flowing.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("20b7f8ce-6954-70cd-c02f-e2e4f8330e78"),
                column: "Explanation",
                value: "ZAP only acts when a verdict changes, and you can't count on its timing while users are clicking.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("28d290c1-67d7-3438-b582-c4cb284ed62c"),
                column: "Text",
                value: "Share permissions override NTFS permissions");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2d5856ff-6304-b4a3-617f-e89b79261b65"),
                column: "Explanation",
                value: "The ARP table shows local IP-to-MAC mappings and tells you nothing beyond the LAN.");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("37565c03-96de-a4bc-caac-d9e509142ab9"),
                column: "Text",
                value: "A completed MFA registration");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3a3d9577-8936-8ded-7251-0257cd724c08"),
                column: "Text",
                value: "Whether the disclaimer text is over a length limit, or contains HTML that the rule can't insert into the vendor's messages");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("40cce026-bde9-b83d-6a2d-88d6a040921e"),
                column: "Text",
                value: "DNS caching delayed the new certificate from reaching the Macs");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("426294d2-2f48-c7fd-af81-77baa64d0e6b"),
                column: "Text",
                value: "Fabrikam's own tenant has a cross-tenant access policy that blocks B2B collaboration with outside organizations");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4837be8e-c4f9-ce45-cec0-3d498221ef82"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "802.1X happens before an IP address is assigned, and these drops are authentication failures.", "Check the DHCP lease for each Mac on the staff VLAN" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("489c3817-f7c6-0a14-808b-e00816f438af"),
                column: "Text",
                value: "Ask the DNS team to lower the record's TTL and wait for the old answer to age out of the cache");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4e8947af-7831-58aa-1bc3-2f09b7817d4d"),
                column: "Text",
                value: "Ping the domain controller's IP address");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5f2f4070-04f6-deb2-80b5-f4c6633b7e51"),
                column: "Text",
                value: "The gateway's ARP cache is full and is blocking new devices");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6ec397f0-5958-26ec-3d74-3673c533c591"),
                column: "Text",
                value: "Check the RTP packet statistics for the failed call on the PBX to see where audio stopped flowing");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7250e01a-749d-f73d-2146-d0bc98c9fae2"),
                column: "Text",
                value: "The jitter buffer on the phones is too small for this WAN link");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7afbc17a-757b-70c9-1e15-6decf95f63f5"),
                column: "Text",
                value: "Check the vendor's SPF record");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("81d30037-002b-6cf2-806f-a882ffb8fa80"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "DNS resolves names and has no part in assigning a client's IP address.", "A DNS server has handed out an address from the wrong zone" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8efd53bc-b6e1-67a8-001d-2e9d1a9c2043"),
                column: "Text",
                value: "The destination is hiding its true loss by blocking ICMP");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9504817a-86de-bed4-2de7-6f616fbd4228"),
                column: "Text",
                value: "The DNS Server log on a domain controller");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("99e42082-f570-da27-e005-d0f238a95d15"),
                column: "Text",
                value: "Open TCP port 5060 inbound from the hosted PBX to the phones so their audio can reach the branch");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a2cc52b8-32bc-fb47-8faa-f9b6be9c06ed"),
                column: "Text",
                value: "The domain's SPF record is failing");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a46b90b1-cb89-d9e0-c063-acd68f8f905b"),
                column: "Text",
                value: "Assign static IP addresses to each device that can't connect");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a4c552d0-a928-8e53-1f0a-444167405c29"),
                column: "Text",
                value: "Publish a DMARC policy of p=reject for your own domain");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("aab06f51-5fbd-627f-828b-da4e0ae8ec5f"),
                column: "Text",
                value: "The WMI filter on the Default Domain Policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b0a3677e-d2e3-d465-7341-196900a74d41"),
                column: "Text",
                value: "NTFS permissions are skipped for network access, and they only apply to people signed in at the server itself");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b6326f9b-e01d-bd56-b5fd-6ef99ede8435"),
                column: "Text",
                value: "The PBX searched its dial plan, found no such extension, and rejected the call with a timeout");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c491f14c-8826-4811-4196-b996ec9498bd"),
                column: "Text",
                value: "SIP registration is failing intermittently at the branch");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c8450ca1-8111-3521-f02c-798ba267e769"),
                column: "Text",
                value: "Take ownership of the folder from TrustedInstaller, then turn on NTFS compression for it from the Properties page");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c858e2cb-1469-9c66-afb5-356c113f6c6c"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "Missing QoS markings lower priority under congestion. They don't remove one direction of audio completely.", "DSCP markings are missing from the voice traffic, so the ISP is dropping the return RTP packets" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d25780cf-6fae-db32-e489-dcc369b881c8"),
                column: "Text",
                value: "Raise the WAN interface MTU to 9000 to improve throughput");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e3cdfc1b-fcfc-413e-38d5-35e46137a36e"),
                column: "Text",
                value: "The default gateway is performing NAT for the guest VLAN");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e40a12c1-8d60-2324-7be9-d0af906cb580"),
                columns: new[] { "Explanation", "Text" },
                values: new object[] { "Ping proves basic reachability with ICMP. It says nothing about which addresses and ports RTP is using.", "Run a sustained ping from a phone's VLAN to the hosted PBX address" });

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ebea3b74-0c06-f68b-f8f2-e769ed88a873"),
                column: "Text",
                value: "The client was given a reserved address by the DHCP server because its MAC is in the exclusion list");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("eda27977-a3ce-a622-4683-d0b6c34ead2a"),
                column: "Text",
                value: "The caller failed digest authentication on the PBX");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("efb5fb9e-3a68-978c-7dde-2a305c369a36"),
                column: "Text",
                value: "The VM had been off for more than 30 days, so Active Directory expired its machine account password");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f52ff85e-dabb-6c0c-5d51-8f0404c13381"),
                column: "Text",
                value: "The revert gave the VM a different IP address");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fbad0072-7ed7-c0b0-b4b4-3f0a2033a9be"),
                column: "Text",
                value: "The SRTP keys don't match, so the phone is discarding the audio");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("0030a01b-87b2-eebf-b546-93c88c9cbac3"),
                column: "TechnicalExplanation",
                value: "A full system drive stops updates, logging, and sometimes the applications themselves, so act quickly, but measure before deleting anything. List the largest folders first. On servers the space usually goes to logs that nobody rotates, such as IIS logs under C:\\inetpub\\logs\\LogFiles, along with dump files, temp folders and old installers. The WinSxS folder is the Windows component store. It holds the files Windows needs to service itself, and Microsoft warns that deleting files from it can leave a system unable to boot or update. Reduce it with the built-in tools. Dism /Online /Cleanup-Image /AnalyzeComponentStore reports how much can be reclaimed, and /StartComponentCleanup removes superseded component versions straight away, where the scheduled task waits 30 days. Adding /ResetBase removes every superseded version, and after that the updates already installed can't be uninstalled. Then fix the cause: put retention on the logs or move them to a data volume, and alert on free space well before it's critical.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Every desk phone at a branch office can place and answer calls through the company's hosted PBX, but on all calls the staff hear nothing while the people they call hear them clearly. Phones show as registered, and the problem began after the branch firewall was replaced.", "SIP sets up a call, and RTP carries the audio as a separate UDP stream, so a call can connect perfectly with audio failing in one direction. Each side uses the SDP body in the SIP messages to announce the IP address and port where it wants to receive RTP. When a phone behind NAT advertises its private address, the far end sends audio to an address it can't route to. This is the classic one-way audio pattern, and it fits here because the new firewall changed how NAT was handled. Fix it with proper NAT traversal, such as STUN, or an SBC or PBX that uses symmetric RTP. Also disable SIP ALG if the firewall's rewriting is making things worse, and make sure the RTP UDP port range is allowed. Compare captures from both sides of the firewall, because they show whether a stream was never sent or was lost on the way." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76"),
                column: "TechnicalExplanation",
                value: "A SIP 408 means a request got no final response within the allowed time. For an INVITE over UDP, Timer B expires at 64 times T1, which is 32 seconds with the default 500 ms T1, and that matches this ticket. Retransmissions with no provisional response show the INVITE isn't reaching the phone, or the phone's reply isn't getting back. Outbound calls work because the phone opens its own path through the router. Inbound calls depend on a NAT or firewall UDP mapping staying open. When the mapping expires between registrations, the PBX's registration is still valid, but its INVITE is dropped at the router. Keep-alives sent more often than the router's UDP timeout hold the mapping open. Avoid changing SIP timers, because a longer wait just delays the same failure.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("342cc552-23d1-4282-8205-22cb79b47a8e"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "At 10:40, three users report an email titled 'Payroll update required' that links to a fake Microsoft sign-in page. It came from hr-notice@payroll-portal.example and passed SPF and DKIM for that domain. The tenant has Microsoft Defender for Office 365 Plan 2.", "Threat Explorer in Defender for Office 365 Plan 2 finds every copy of a campaign by sender, subject or URL, and shows each copy's delivery location along with URL clicks recorded by Safe Links. You can act on the results directly, for example with a soft delete, and the Action center tracks the remediation. Zero-hour auto purge (ZAP) can also remove delivered messages when a verdict changes, but you can't rely on its timing during an active campaign. A sender that passes SPF and DKIM for its own lookalike domain isn't spoofing you, so your own DMARC policy won't stop it, and the Tenant Allow/Block List is the place to block the domain and the URL. Anyone who clicked through to the credential page may have given away a password, so reset it, revoke their sessions and review their sign-ins." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("3735d78e-3fdf-e9bc-84ba-41b5d2225e0e"),
                column: "TechnicalExplanation",
                value: "A shared folder has two sets of permissions. Share permissions apply only to access over the network, and NTFS permissions apply to every kind of access. Over the network both are evaluated and the more restrictive result wins, so Full Control on the share with Read in NTFS gives Read. Common practice is to keep share permissions broad, for example Authenticated Users with Full Control or Change, and to do the real control in NTFS with groups. Within one set, a user's permissions from all their groups add up, but a Deny entry beats any Allow. The Effective Access tab under Advanced Security Settings calculates the NTFS result for a named user. Group membership has its own catch. A user's access token, which lists their groups, is built when they sign in. Adding someone to a group changes Active Directory, but not a token that already exists, so the user has to sign out and back in. whoami /groups shows the groups in the current token. Grant access to groups and not to individual accounts, or the permissions become unmanageable.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("5ad4d533-6843-622d-0c9a-4dfb3261a2eb"),
                column: "TechnicalExplanation",
                value: "Domain controllers copy changes to each other, and when that stops, each site slowly works from a different version of the directory. Start with repadmin /replsummary for a forest-wide view of failures and how old they are, then repadmin /showrepl on the failing DC for each partition's last attempt, result, and last success. Error 1722, 'The RPC server is unavailable', is a connectivity error: the destination DC couldn't connect to the source DC. Microsoft notes that DNS lookup failures cause a large share of these, because DCs locate each other through records in the _msdcs zone. dcdiag /test:dns checks that, and nltest /dsgetdc tests the DC locator. A DC must use internal DNS servers that host the Active Directory zones. A public resolver doesn't hold those records, and an alternate server is only tried when the preferred one doesn't answer. Other causes of 1722 are blocked ports, namely TCP 135 for the endpoint mapper and the dynamic RPC range. Don't leave replication broken for long. A DC that stays out of sync past the tombstone lifetime, usually 180 days, can bring deleted objects back.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("652a4793-984d-cf69-18d2-336dda12226c"),
                column: "TechnicalExplanation",
                value: "An account locks when the count of bad passwords passes the lockout threshold, so a lockout that repeats on a timer means something is still sending the old password. A domain controller that sees a bad password checks it with the PDC emulator, which always has the newest password, and lockouts are processed there. That makes the PDC emulator's Security log the place to look for event 4740, 'A user account was locked out'. Find that DC with (Get-ADDomain).PDCEmulator. The event's Caller Computer Name field names the computer the failed attempt came from. On that computer, event 4625 records each failed logon with its logon type: 2 is interactive, 3 is network, 4 is batch (scheduled tasks), 5 is service, and 10 is Remote Desktop. Usual culprits are scheduled tasks, services, mapped drives, saved entries in Credential Manager, disconnected Remote Desktop sessions, and mail apps on phones. Event 4740 is only logged when Audit User Account Management is enabled.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Users at a branch office report choppy video calls and stalls in a cloud app at 203.0.113.50, starting this morning. Everything on the local file server works normally. You're on a Linux machine on the branch LAN.", "Ping is the quickest way to scope a problem: test the default gateway first, then an external target. MTR then combines ping and traceroute by sending a stream of probes to every hop. The rule for reading it is that loss only matters if it continues through to the final destination. A router in the middle showing loss when the hops after it don't is usually just rate limiting the ICMP or TTL-expired replies it generates, and it's forwarding traffic fine. Real loss starts at one hop and carries through to the end, which points to that hop or the link leading to it. Paths are often asymmetric, so run MTR from the far side too if you can, and check interface errors and utilization on your own equipment before escalating. Give the ISP timestamped MTR reports from both directions." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("6f67ae98-4a03-2943-e2d3-cd7ab58e559a"),
                column: "TechnicalExplanation",
                value: "NDR 5.7.520 means 'Access denied, your organization does not allow external forwarding'. Automatic external forwarding is controlled by the outbound spam filter policy, whose default setting (Automatic - System-controlled) behaves as Off. Attackers often use forwarding rules to exfiltrate mail from compromised mailboxes, so leave the Default policy alone and enable forwarding only through a custom policy scoped to the people with an approved business need. Check any remote domain settings as well, because they can also stop automatic forwarding.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("8fed5b49-1c48-68d0-d6c5-806475f0323f"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "A colleague built a GPO called Sales Drive Map with user settings and linked it to the Staff OU. To limit it to the sales team, they removed Authenticated Users from Security Filtering and added the Sales-Users group. Nobody in Sales gets the drive, and gpupdate /force doesn't help.", "A GPO applies to a user only if it is linked where the user's account lives, and the user has both Read and Apply group policy permissions on it. Security Filtering is a shortcut for granting those two permissions. There's one more requirement that catches people out. Since security update MS16-072 in June 2016, Windows fetches user policy with the computer's security context, where it used to use the user's. The computer account therefore needs Read permission on any GPO that applies to the signed-in user. By default it gets that through Authenticated Users, a group that includes computer accounts. Removing Authenticated Users from Security Filtering takes away Read as well as Apply. The fix is on the GPO's Delegation tab: add Authenticated Users or Domain Computers with Read only. That lets computers read the GPO without applying it to everyone. On the client, gpresult /h produces a report of which GPOs were applied or denied, and why. A Deny entry always beats an Allow." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("95e42524-b675-8aab-3bae-f938012f09ed"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Your team moved intranet.corp.example to a new server this morning and updated DNS. Windows laptops reach the new site, but a MacBook still loads the old server. The Mac has a normal internet connection, and browsing elsewhere works fine.", "macOS caches DNS results in the mDNSResponder daemon, and most apps resolve names through that system resolver. Tools such as dig and nslookup query DNS servers directly and skip the system resolver, while dscacheutil -q host follows the path that apps use. That makes comparing them a good test: if dig shows the new address and dscacheutil shows the old one, the fault is on the Mac rather than in DNS. Clear it with sudo dscacheutil -flushcache followed by sudo killall -HUP mDNSResponder. The HUP signal tells the daemon to drop its cache without stopping it. If the old answer survives a flush, look at what the system resolver checks besides DNS, such as an entry in /etc/hosts, or a scoped resolver from a VPN, which scutil --dns will list." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c83bf57c-c873-bf4a-6940-737ab5c9cd24"),
                column: "TechnicalExplanation",
                value: "This error means the client never reached a working RDP listener, and Microsoft's guidance splits the causes into the listener and the network. With console access, run qwinsta on the server. A line for rdp-tcp in the Listen state means the listener is up. If it's missing, check that fDenyTSConnections is 0, both under HKLM\\SYSTEM\\CurrentControlSet\\Control\\Terminal Server and in the policy path if one is set, and that the Remote Desktop Services service (TermService) is running. Next, connect from the console with mstsc to localhost. If that works, the server side is healthy and the fault is between client and server. Test-NetConnection with -Port 3389 from another machine tests the TCP port itself, which ping can't do because ping uses ICMP. A False result points to the Windows Firewall rules named Remote Desktop - User Mode (TCP-In and UDP-In), a network firewall, or a changed port number. Failures after that point, such as credentials, Network Level Authentication, certificates, or membership of Remote Desktop Users, produce different messages because the connection has already been made.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c892bf25-98a8-f270-b2c2-9ccd15cc1750"),
                column: "TechnicalExplanation",
                value: "SharePoint and OneDrive sharing works in layers. The organization-level setting in the SharePoint admin center is the most permissive level allowed, and each site can match it or be more restrictive. Domain restrictions apply on top of both, either for the whole organization under Policies > Sharing ('Limit external sharing by domain') or for a single site. Sharing with a domain that isn't on an allow list fails with this policy message, which is why one partner works and another doesn't. SharePoint also uses Microsoft Entra B2B for guests, so an allow or deny list in the Entra external collaboration settings can block the invitation too, and the two lists should agree. The narrow fix is adding the approved domain to the allow list, not loosening the sharing level or removing the restriction for everyone.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "MacBooks on the WPA2-Enterprise SSID CorpSecure started dropping off Wi-Fi about 8 hours after connecting, and they fail to rejoin. It started the day after the RADIUS server's certificate was renewed. Windows laptops are fine. The Macs connect using a configuration profile from your MDM.", "802.1X authenticates a device before it's given network access. With PEAP or EAP-TLS, the client validates the RADIUS server's certificate during the TLS handshake. On macOS, the eapolclient process handles this, and an MDM profile normally defines which certificates and server names to trust. Macs stay connected on an existing session, then fail when reauthentication happens, which explains drops long after the certificate change. Here the 8-hour pattern matches the session timeout. Windows machines were fine because they trust the new chain another way. The right fix is to push an updated profile containing the new CA chain and trusted server names. Telling users to click Trust or turning off validation trains people to accept a fake access point, which is how credentials get stolen. On Windows NPS, event 6272 means access was granted and 6273 means it was denied." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("d4b16ac7-5976-0af5-8fbd-2d0d1bb56417"),
                column: "TechnicalExplanation",
                value: "Every domain-joined computer has its own account in Active Directory, with a password that the computer and the domain both know. The computer uses it to build a secure channel to a domain controller, and domain sign-ins on that computer depend on the channel. By default the computer itself starts a password change every 30 days. Active Directory never expires a machine password, so a PC that was simply switched off for months still works. The usual real causes are restoring an old checkpoint or image, which brings back a password the domain has since replaced, or a computer account that was reset, deleted, or taken over by another machine with the same name. Test-ComputerSecureChannel reports whether the channel is healthy, and adding -Repair with a domain credential resets it in place. Reset-ComputerMachinePassword is the next option. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort. Don't delete the computer object, since a new one has a different SID and loses its group memberships.");

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("dc625090-483f-4846-95a4-f7c8b8d9fcc9"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "Luis from Sales is at a client site in Lisbon and can't sign in to Outlook or Teams. The browser shows error AADSTS53003, 'Access has been blocked by Conditional Access policies'. No MFA prompt appears, and the same account worked from the office yesterday.", "Microsoft Entra ID evaluates every sign-in against the Conditional Access policies that apply to it, and the sign-in log records the result of each policy. AADSTS53003 means a policy's grant control was Block, which is why no MFA prompt appeared: a block is applied before any challenge is offered. Country-based named locations are resolved from the sign-in's public IP address, so a user abroad falls outside an 'allowed countries' location. The supported way to make an exception is an exclusion group with a documented end date, ideally governed by an access review. Disabling the policy, switching it to Report-only, or trusting a hotel IP weakens protection for everyone. The What If tool evaluates a simulated sign-in against every policy, so you can prove the exception works before the user tries again." });

            migrationBuilder.UpdateData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c"),
                columns: new[] { "ProblemStatement", "TechnicalExplanation" },
                values: new object[] { "At 9 AM, new devices on the staff and guest VLAN 20 (192.168.20.0/24) can't get online and show 169.254.x.x addresses. Devices that were already connected still work. The VLAN uses a Windows DHCP server, and a conference has been running at the office all week.", "A 169.254.x.x address is an APIPA link-local address, which a client assigns itself when no DHCP server offers a lease. Devices that already hold a lease keep working, which is the classic sign of exhaustion rather than a server outage. A /24 has only 254 usable addresses, and a Windows DHCP scope defaults to an 8-day lease, so visitors' phones keep addresses long after leaving. Phones using private (randomized) MAC addresses can make this worse, since one device can look like several clients. Shorten the lease on busy guest networks, and reclaim stale leases. Changing the duration only affects new leases and renewals, so it needs some time or lease cleanup. Turn on conflict detection while reclaiming, since a device might still be using a removed address. Then fix capacity properly by widening the subnet or moving guests to their own VLAN." });

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
                column: "Prompt",
                value: "Where do you grant it in the admin UI?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"),
                column: "Prompt",
                value: "dig returns the new IP, and dscacheutil returns the old one. What does that show?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("0c432c4d-2b22-b689-6f3a-b783aa2ce4f7"),
                column: "CorrectAction",
                value: "The policy blocked the sign-in because Portugal isn't in the allowed countries location");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1f27afdc-f222-3873-de10-acdd29d0f531"),
                column: "Explanation",
                value: "The permissions are right. The share lets any authenticated user through with Full Control, and NTFS gives Modify to Finance-RW, so NTFS decides and members get Modify. Tomas is a member in Active Directory, but whoami /groups shows his current access token, and the group isn't in it. Windows builds the token once, at sign-in, from the groups he had at that moment. He signed in this morning and was added an hour ago. The file server checks the token, finds no Finance-RW, and denies access.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2797eeb9-d549-3804-7e57-f8a581231baf"),
                column: "Prompt",
                value: "You find 38 GB of IIS logs going back four years in C:\\inetpub\\logs\\LogFiles, and a 14 GB WinSxS folder. What is the best first move?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"),
                column: "Explanation",
                value: "Over UDP, SIP resends a request that gets no response, doubling the wait each time: 0.5, 1, 2 and 4 seconds. A phone that receives an INVITE answers almost at once with 100 Trying, then 180 Ringing. Neither appears, so the INVITE never arrived, or the answers were lost. After 32 seconds, which is Timer B, the PBX gives up and returns 408 to the caller.");

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
                keyValue: new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"),
                column: "CorrectAction",
                value: "Confirm the profile is installed, then verify NPS event 6272 through a full session timeout");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("46baf750-afaa-a6df-25f8-be25c4030396"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "BR-DC02 can't connect to the RPC service on its source DC, so suspect DNS or the network path", "Replication is pull-based. BR-DC02 connects to HQ-DC01 over RPC and asks for changes. Error 1722, 'The RPC server is unavailable', means that connection couldn't be made. The causes are a name that doesn't resolve, a blocked port (TCP 135 and the dynamic RPC range), or a source server that is down. HQ-DC01 is serving headquarters, so look at DNS and the path between the sites." });

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
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Ignore hop 5, since loss that doesn't carry on is likely ICMP rate limiting", "Each line of MTR is that router's reply to probes addressed to it, and routers give those replies a low priority and often rate limit them. Traffic passing through a router is handled separately. If hop 5 were dropping forwarded packets, hops 6 to 9 would show loss as well, because their probes pass through it. They show 0%, so hop 5 is forwarding fine. The rule is that loss only counts when it continues to the final hop." });

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
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Share permissions first, then NTFS permissions, and he gets only what both sets allow", "A network connection to \\\\FILE-SRV01\\Finance passes two gates. First the server checks the share permissions on the Finance share, which only apply to access over the network. Then it checks the NTFS permissions on the folder itself, which apply to every kind of access. Windows works out his total in each set from all his groups, and the more restrictive of the two totals is what he gets. Full Control on the share with Read in NTFS gives Read, and Read on the share with Modify in NTFS also gives Read. For this ticket it means a block in either set is enough to cause 'Access is denied', so you have to look at both." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5dcf5928-cfee-73c7-2963-82771c2fa035"),
                column: "Explanation",
                value: "repadmin /replsummary contacts every domain controller and prints one line each, as a source and as a destination: how many replication attempts failed, the longest time since a success, and the error code. It shows in seconds which DC is behind and for how long. repadmin /showrepl on that DC then gives the detail for each partition and partner.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("61b306d2-dcbe-00e0-cb44-f92732a4ee51"),
                column: "CorrectAction",
                value: "The mailbox is probably compromised, and an attacker created the rule to hide the bank's replies");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6b5fcfe4-9e73-5b7b-3be8-5a3b7d9609bd"),
                columns: new[] { "CorrectAction", "Prompt" },
                values: new object[] { "Point BR-DC02 back at the internal AD DNS servers, then clear its resolver cache", "The DNS test fails. BR-DC02's network adapter lists 8.8.8.8 as its preferred DNS server, set during an ISP outage last week. What is the fix?" });

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
                column: "CorrectAction",
                value: "Short-stay devices are holding addresses for 8 days, draining a small /24 scope");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "SIP signaling is working, so the fault is in the RTP media path", "A VoIP call uses two protocols. SIP is the signaling that makes the phone ring and connects the call. RTP carries the audio, as a separate UDP stream in each direction, on different ports. These calls ring and connect, so SIP works. One direction of audio is missing, so one RTP stream isn't arriving. That narrows the fault to the media path, and the new firewall is the obvious suspect." });

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
                column: "Prompt",
                value: "A later MTR shows 0% at hops 1 and 2, then 6% at hop 3, and about 6% on every hop through to the destination. Hop 2 is your firewall, and hop 3 is the ISP's gateway. What does this show?");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("72bf031d-2d13-1a34-549a-b71b4faffb68"),
                column: "Explanation",
                value: "Quarantined means Exchange Online Protection or Defender for Office 365 judged the message unsafe and held it, so it never reached any mailbox folder and the sender got no bounce. In the Defender portal, go to Email & collaboration > Review > Quarantine. Check the sender, the authentication results and a preview before releasing, because an invoice is exactly what a phishing email imitates.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("80fd1395-920b-e2ed-69ae-a7d58585b232"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "User policy is now fetched as the computer, and no entry lets the PCs read this GPO", "Since the MS16-072 update in 2016, Windows downloads user policy using the computer account, where it used to use the user's. So the computer needs Read on the GPO. Normally it has that through Authenticated Users, which includes computer accounts. Removing that group from Security Filtering took away Read as well as Apply. Nothing in this list covers the sales PCs, so they can't read the GPO, and it's skipped." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"),
                column: "Explanation",
                value: "The system resolver has sources other than DNS, and a flush doesn't touch them. The file /etc/hosts is read first, so a line for intranet.corp.example there, perhaps left from testing, overrides DNS permanently. scutil --dns lists every resolver the Mac uses, including ones a VPN adds for specific domains, which may point at a DNS server that still has the old record.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("941e7c17-066e-1729-ea54-2d58af65c26d"),
                column: "Explanation",
                value: "Active users > Add a user creates the Entra ID account and walks through the basics, then the usage location and license, then optional roles and profile details, in one pass. The mailbox and other services are provisioned once the license is assigned. For many users at once you'd use a CSV import or PowerShell, but for one new hire the wizard is the standard route.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("981d38df-be64-d403-71f2-931cdbd8145e"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Have Tomas sign out and sign back in, so that he gets a new token", "A new sign-in makes Windows build a new access token and get new Kerberos tickets, and those include Finance-RW. Nothing on the server needs to change, because the permissions were already correct. Afterwards, whoami /groups should list Finance-RW, and the share should open. Running klist purge can refresh the tickets without a sign-out, but signing out and in is the reliable answer." });

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
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Sign in with a local administrator account", "A local account is stored on the PC itself, in the SAM database, and it's verified there without any domain controller. That's why it still works when the secure channel is down. Use the built-in Administrator or a local admin managed by LAPS, signing in as .\\Administrator so Windows checks the local database." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Ping the default gateway, then an external IP, each with about 100 packets", "Start by finding out which side of the gateway the loss is on. Ping the default gateway to test the LAN, then an external address to test the path beyond it. Use around 100 packets, for example ping -c 100, because the result is a percentage and a few packets can easily miss occasional loss. A clean gateway with loss outside points to the WAN." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a36f3e7c-c851-b3ce-5b5f-8261ccd91317"),
                column: "CorrectAction",
                value: "The usage location for the user");

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
                column: "CorrectAction",
                value: "Turn on the setting that allows people outside the organization to email the group");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ac5dd38e-1a07-400b-ccc4-1234a88947fc"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "Find what is still sending the old password before unlocking the account again", "An account locks when bad password attempts reach the lockout threshold. Dana isn't at her desk when it happens and it repeats every 20 minutes, so a device or program is sending her old password on a schedule. Unlocking resets the counter, and the next few attempts lock it again. The real job is to find the source, and the logs can name it." });

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
                keyValue: new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"),
                columns: new[] { "CorrectAction", "Explanation" },
                values: new object[] { "The client got no DHCP offer and assigned itself a link-local APIPA address", "A client gets its address by broadcasting a DHCP Discover and waiting for an Offer. If no offer comes, Windows and macOS give themselves an address in 169.254.0.0/16, called APIPA or link-local. It only works on the local segment and has no gateway. So this address always means the client asked for a lease and got no answer." });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ba1f52d7-0140-810e-9011-86be84ffcc04"),
                column: "Explanation",
                value: "Test from the inside outwards. mstsc to localhost on the server uses the RDP service without crossing the network or the firewall, so if it works, the service is healthy. Then, from another PC, Test-NetConnection FILE-SRV03 -Port 3389 tries a TCP connection to the RDP port. TcpTestSucceeded: True means the port is reachable, and False means something is blocking it. Ping can't answer this, because it uses ICMP and not TCP 3389.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"),
                column: "Explanation",
                value: "The far end needs a public address and port to send audio to. STUN lets the phone learn its public address and put it in the SDP. Or the provider's SBC can use symmetric RTP, where it ignores the SDP address and replies to wherever the phone's audio came from. The firewall must also allow the provider's RTP UDP port range. If SIP ALG on the new firewall is rewriting packets badly, turn it off.");

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
                keyValue: new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"),
                columns: new[] { "Explanation", "Prompt" },
                values: new object[] { "SIP response codes follow the same pattern as HTTP. 4xx is a request failure, and 408 Request Timeout means a request was sent and no final response came back in time. It doesn't say why. A 408 is generated by whichever device gave up waiting, so the work is to find how far the INVITE got.", "What does SIP 408 Request Timeout tell you?" });

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
                column: "CorrectAction",
                value: "The Security log on the PDC emulator, filtered to event 4740 for Dana's account");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f16d6501-0c6d-1f56-ec85-145fab0ff024"),
                column: "Explanation",
                value: "With -Repair, the cmdlet sets a new machine password and writes it to both the PC and its account in Active Directory, using the domain credential you supply. The computer object, its SID and its group memberships stay as they are, and normally no restart is needed. Reset-ComputerMachinePassword is the alternative. Leaving and rejoining the domain also works, but it takes two restarts and is the last resort.");

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"),
                columns: new[] { "Explanation", "Prompt" },
                values: new object[] { "In PEAP and EAP-TLS, the RADIUS server presents its certificate first, and the client checks it before sending any credentials. The Mac's configuration profile lists exactly which CA and server names to trust. The renewed certificate comes from a chain that isn't in that list, so the Mac ends the handshake with an unknown CA alert. The password is never sent, so it can't be the problem.", "The Mac logs show a TLS failure during EAP, and RADIUS logs an unknown CA alert from the client at each drop. Passwords are correct. What is the cause?" });

            migrationBuilder.UpdateData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f430db22-4555-1b5b-092d-f0e293522887"),
                column: "CorrectAction",
                value: "Whether that label's group settings stop owners from adding guests");
        }
    }
}
