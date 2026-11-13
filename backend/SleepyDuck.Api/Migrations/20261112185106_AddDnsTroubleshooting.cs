using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDnsTroubleshooting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScenarioCards",
                columns: new[] { "Id", "Category", "ProblemStatement", "TechnicalExplanation", "Title" },
                values: new object[,]
                {
                    { new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80"), "DnsTroubleshooting", "Remote Desktop to PC-ACCT-07 by name fails for the help desk, and a ping to the name gets replies from an address that belongs to a different laptop. PC-ACCT-07 was reimaged last week, when its old computer account was deleted and the PC was joined to the domain again. PCs get their addresses from DHCP (Dynamic Host Configuration Protocol) and register themselves in the corp.example DNS (Domain Name System) zone.", "Windows PCs register their own A (address) records in DNS (Domain Name System) through dynamic update, and re-register them every 24 hours and whenever the address changes. A record goes stale when that update stops or is refused, and the name then points at an address that DHCP (Dynamic Host Configuration Protocol) has since leased to another device. Confirm it by comparing three things: the address DNS returns (nslookup), the address the PC really has (ipconfig), and who holds the address now (the DHCP lease list, or nslookup on the address for its PTR (pointer) record). On an Active Directory-integrated zone set to secure dynamic updates, each record has an ACL (access control list), and only the account that created the record may change it. A PC rejoined with a new computer account has a new SID (security identifier), so its updates to the old record are refused, and the DNS client logs event 8015 in the System log. Delete the stale record and run ipconfig /registerdns on the PC so that it creates a new one that it owns. For the long term, turn on aging and scavenging, which deletes dynamic records that haven't been refreshed. It has to be enabled on the zone and on at least one DNS server, and the default no-refresh and refresh intervals are 7 days each. Static records have no timestamp and are never scavenged.", "Hostname resolves to the wrong computer" },
                    { new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703"), "DnsTroubleshooting", "Since the company moved to a new VPN (virtual private network) client, remote staff can't open \\\\files01 or http://intranet. The same resources open in the office, and they open over the VPN when someone types the full name, such as files01.corp.example. The laptops aren't joined to the domain.", "DNS (Domain Name System) only holds FQDNs (fully qualified domain names). When a user types a single-label name such as files01, the Windows DNS client turns it into an FQDN by appending suffixes, then queries each result in turn. The suffixes come from the primary DNS suffix, which a domain-joined PC gets from its domain, from the connection-specific suffix of each adapter, set by DHCP (Dynamic Host Configuration Protocol) or by a VPN (virtual private network) profile, or from a DNS suffix search list, which replaces the other two when it is set. ipconfig /all shows all three. If none of them supplies corp.example, no query for files01.corp.example is ever sent. Windows then falls back to LLMNR (Link-Local Multicast Name Resolution) and NetBIOS broadcasts, which only reach the local network and don't cross a VPN. The test is to resolve the short name and the full name side by side with Resolve-DnsName. If only the full name works, the server and the record are fine and the client's suffixes are wrong. Fix it where the configuration comes from: the VPN profile for remote clients, DHCP option 15 for an office network, or the DNS suffix search list setting in Group Policy for domain PCs.", "Short names fail but full names resolve" },
                    { new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072"), "DnsTroubleshooting", "At 8 AM the whole office reports that 'the internet is down'. Websites won't load, but the intranet and file shares work. The PCs use the internal Windows DNS (Domain Name System) server at 10.0.0.10, and the office changed ISP (internet service provider) last night.", "A name failure looks like an outage to users, so separate connectivity from resolution first. A ping to a public IP (Internet Protocol) address that replies proves the path is up, and nslookup then tests DNS (Domain Name System) by itself. nslookup prints the server it asked, and a second argument makes it ask a different one, as in nslookup www.microsoft.com 1.1.1.1, so one command compares your server with a public resolver. An internal DNS server answers for its own zones from its own data. For every other name it either forwards the query to the resolvers on its Forwarders list or works down from the root servers using root hints. If the forwarders stop answering and root hints are turned off, internal names keep working and everything external fails. ISP (internet service provider) resolvers usually answer only that ISP's customers, so they are a common casualty of a provider change. Fix the Forwarders list on the server with Set-DnsServerForwarder, clear its cache with Clear-DnsServerCache, and test. Don't point clients at a public resolver to get round it, because it can't answer for internal zones and domain sign-in depends on those.", "External names fail while internal names resolve" },
                    { new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85"), "DnsTroubleshooting", "At 2 PM the company moved www.corp.example to a new web host and changed its A (address) record at the DNS (Domain Name System) provider. Two hours later some customers reach the new site, while the office and other customers still get the old server. The record's TTL (time to live) was 86400 seconds.", "Every DNS (Domain Name System) record carries a TTL (time to live), the number of seconds a resolver may keep the answer before asking again. Only the authoritative name servers hold the real record. Everyone else gets it through a recursive resolver, which serves its cached copy until the TTL runs out. What people call propagation is those caches expiring at different moments, which is why some users see the new address hours before others. To troubleshoot, ask the authoritative server directly: nslookup -type=NS corp.example lists the name servers, and nslookup www.corp.example followed by one of them returns the record as published. If that is right, the change worked and you are waiting on caches. The TTL in a resolver's answer counts down, so it shows how long that resolver will keep the old address. You can clear the caches you run, with Clear-DnsServerCache on a Windows DNS server and ipconfig /flushdns on a PC, but not other people's resolvers. Plan ahead for the next change: lower the TTL to about 300 seconds at least one full old TTL before the change, keep the old server running until the old TTL has passed, and raise the TTL again afterwards.", "Old address served after a record change" }
                });

            migrationBuilder.InsertData(
                table: "SequentialSteps",
                columns: new[] { "Id", "CorrectAction", "CorrectOptionId", "Explanation", "Order", "Prompt", "ScenarioCardId" },
                values: new object[,]
                {
                    { new Guid("12472fe7-e3ca-b1cf-9e83-2aeca7b364c0"), "The Forwarders list, with Get-DnsServerForwarder, testing each address with nslookup", new Guid("b139e07d-8c7b-7ddb-b133-af6101059d0d"), "A Windows DNS server resolves outside names in one of two ways. It forwards the query to the resolvers listed on its Forwarders tab, or it uses root hints to work down from the root servers itself. Get-DnsServerForwarder lists the forwarder addresses and shows whether root hints are used as a fallback. Test each forwarder from the server with nslookup www.microsoft.com followed by its address. A forwarder that times out there is the break.", 4, "The public resolver answers straight away. On the DNS server, what do you check?", new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072") },
                    { new Guid("12869525-01a1-e3d2-9a7e-8d2f62b18c2e"), "PCs can reach the DNS server, but it can't resolve names outside its own zones", new Guid("c8dff5b9-4a7b-710c-2027-944df8bf76d7"), "The intranet answer proves three things: the PC is pointed at 10.0.0.10, the server is reachable, and the DNS service is running. The server holds the corp.example zone itself, so it answers those names from its own data. It holds nothing for microsoft.com, so it has to ask other servers on the internet, and that is the part that's failing.", 2, "nslookup shows Server: 10.0.0.10 and times out for www.microsoft.com. The same command resolves intranet.corp.example at once. What does that tell you?", new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072") },
                    { new Guid("2d6f646c-6cdb-6c21-d3d7-1d52dcb32cba"), "Replace the forwarders with resolvers that answer, clear the server's cache, and test", new Guid("fc376e60-2aed-3398-749a-71683fb54d9a"), "ISP resolvers normally answer only that ISP's own customers, so they stopped responding when the office moved to the new provider. Set-DnsServerForwarder -IPAddress replaces the list, for example with the new ISP's resolvers or public ones such as 1.1.1.1 and 9.9.9.9. Clear-DnsServerCache then drops the failures the server has cached, and nslookup from a PC confirms that external names resolve. The change is made once on the server, and nothing has to be touched on the PCs.", 5, "The forwarders are the old ISP's resolvers, and both time out. The root hints fallback is turned off. What is the fix?", new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072") },
                    { new Guid("567a9711-2995-bc9b-f46c-19a1f539fd82"), "Run nslookup www.microsoft.com 1.1.1.1 to ask a public resolver directly", new Guid("b6eb1ced-040c-af62-af41-b31692c86bbd"), "A second argument tells nslookup which server to ask, so this sends the same question to a public resolver and bypasses 10.0.0.10. If 1.1.1.1 returns an address, the internet link carries DNS traffic and the name exists, which leaves your own server's upstream as the fault. If it also times out, look at the firewall for blocked port 53, on UDP (User Datagram Protocol) and TCP (Transmission Control Protocol).", 3, "How do you prove that the fault is in your DNS server's path to the internet, and not in the internet link itself?", new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072") },
                    { new Guid("5b6ca569-8d04-1904-b86f-d7411c8cae44"), "The record belongs to the deleted computer account, so the new account's update is refused", new Guid("f6b5e7c2-8548-1464-b695-4032b0590f0b"), "With secure dynamic updates, every record in the zone has an ACL (access control list), and only the account that created it is allowed to change it. The old PC-ACCT-07 computer account created this record. Deleting that account and joining the PC again made a new account with a new SID (security identifier), even though the name is the same. The DNS server sees a different account trying to change someone else's record and refuses, and the client logs event 8015 each time it tries.", 3, "The zone accepts secure dynamic updates only, and the System log on PC-ACCT-07 has DNS client event 8015, a failed registration. Why can't the PC fix its own record?", new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80") },
                    { new Guid("5d2fca48-64b3-703d-c447-2f1ac4ef6fd6"), "Delete the stale record in DNS Manager, then run ipconfig /registerdns on PC-ACCT-07", new Guid("5944d791-bcab-7dcc-9a96-2be37a335fd8"), "An administrator can delete any record, so remove the old A record for PC-ACCT-07 from the corp.example zone in DNS Manager. Then ipconfig /registerdns on the PC makes it register straight away. With no old record in the way, the server lets the new computer account create one, and that account owns it from now on. Run ipconfig /flushdns on the help desk PC so that it drops the old answer, and confirm with nslookup that the name returns 10.0.5.87.", 4, "What fixes the name today?", new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80") },
                    { new Guid("67b42063-70d5-8463-b9db-67d929a6b203"), "Clear the caches you control, and keep the old server running until the old TTL has passed", new Guid("85e15fb3-4ffd-c61d-a867-3663e6f5ee02"), "You can only empty caches you run. Clear-DnsServerCache on the office DNS server and ipconfig /flushdns on the PCs make the office fetch the new address straight away. Browsers keep a short cache of their own, so restart them too. Customers' resolvers belong to their ISPs, and they will keep sending people to the old address for up to 24 hours from the change. Keep the old server answering, or have it redirect to the new one, until that time is up.", 4, "The office needs the new site now, and customers are still landing on the old server. What do you do?", new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85") },
                    { new Guid("6edca510-7231-eb8e-485a-4f2ba429ab56"), "The DNS server and record are fine, and the laptop isn't appending corp.example", new Guid("6e4bb9b4-c8fa-1f60-d9d1-bd729b1b54c8"), "DNS stores and answers full names only. The full name resolved, so the VPN is handing out a DNS server that works and the record for files01 is there. For a short name, the client has to build the full name itself by adding a suffix before it sends the query. The laptop isn't adding corp.example, so the question that reaches DNS is for a name that doesn't exist.", 2, "files01.corp.example resolves to 10.0.4.20. files01 returns 'DNS name does not exist'. What does that show?", new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703") },
                    { new Guid("73b1d146-0486-fabd-60aa-2a7f6443355f"), "Lower the TTL to about 300 seconds at least a day before the change, then raise it afterwards", new Guid("42ea86b1-efcd-8f61-c94e-b8cc516accf6"), "The TTL that matters is the one resolvers already hold. Lower it to 300 seconds at least one full old TTL ahead, which is 24 hours here, so every cached copy with the long TTL has expired by the time of the move. After the change, any resolver picks up the new address within five minutes. Once the new site is confirmed, put the TTL back up, since a longer TTL means fewer lookups and some protection if the DNS provider has a brief outage.", 5, "How should the next move be planned so that the switch takes minutes?", new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85") },
                    { new Guid("75a0ff91-ea0d-ef72-7c1a-499eef3c0860"), "Run nslookup www.microsoft.com to test name resolution on its own", new Guid("22c488be-2d7f-506a-099f-fcb7443ecfe4"), "Replies from 8.8.8.8 show that the PC, the gateway and the new ISP link are passing traffic. A browser needs one more thing before it can connect, which is an address for the name. nslookup asks the DNS server for that address and nothing else, and it prints which server it asked. If it fails while a ping to an IP address works, the fault is name resolution and not connectivity.", 1, "On an affected PC, ping 8.8.8.8 gets replies, but www.microsoft.com won't open in a browser. What do you test next?", new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072") },
                    { new Guid("8a984beb-c5b1-6ca1-70ba-6d9c12fc8f59"), "In ipconfig /all, under the primary suffix, the search list and each adapter's suffix", new Guid("da0db1c0-7266-0bee-bbf6-f9471465cb35"), "ipconfig /all prints the three settings that matter. Primary Dns Suffix comes from domain membership. DNS Suffix Search List is the ordered list that the client will try. Connection-specific DNS Suffix appears under each adapter, including the VPN's, and is set by DHCP (Dynamic Host Configuration Protocol) or by the VPN profile. Get-DnsClientGlobalSetting and Get-DnsClient show the same values in PowerShell.", 3, "Where do you see which suffixes the laptop adds to a short name?", new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703") },
                    { new Guid("9afc475b-4e56-12d7-8f35-801d415db20b"), "Compare the address from nslookup PC-ACCT-07 with the address ipconfig shows on that PC", new Guid("188dce97-42bc-26e2-5da5-232e8d27ad37"), "Two facts are needed: the address DNS gives out for the name, and the address the PC really has. nslookup PC-ACCT-07 returns the first, and ipconfig on the PC itself, or its lease in the DHCP console, gives the second. If they differ, the record is wrong. The DHCP lease list, or a reverse lookup with nslookup followed by the address, then shows which device holds that address now.", 1, "How do you confirm that DNS is pointing the name at the wrong machine?", new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80") },
                    { new Guid("a8370a9a-c31f-427c-4d5c-04d2a8ba31ec"), "Ask one of the domain's authoritative name servers for www.corp.example directly", new Guid("d3c0b973-782d-8faf-7590-aaf8e945f0a8"), "The authoritative name servers are the ones the DNS provider runs for corp.example, and they hold the record as it is actually published. nslookup -type=NS corp.example lists them, and nslookup www.corp.example ns1.dnshost.example asks one of them directly, with no cache in between. This tells you whether the change was saved correctly before you spend time anywhere else.", 1, "What is the first thing to verify?", new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85") },
                    { new Guid("ab953f6a-d292-5339-39a3-4846d269351a"), "Resolve-DnsName files01, then Resolve-DnsName files01.corp.example", new Guid("80b80f1f-63a8-8288-c9c6-d12e682fdbd6"), "The report already hints that short names fail and full names work, so test exactly that. Resolve-DnsName uses the Windows DNS client, the same path that File Explorer and browsers use. Running it with both forms of the name shows whether the VPN's DNS server can be reached and holds the record, and whether the laptop can turn the short name into the full one.", 1, "A remote user is connected to the VPN. What comparison do you run on the laptop first?", new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703") },
                    { new Guid("cdaec7f1-7b9e-ed86-a3e1-0d0890937c06"), "The office resolver is serving a cached answer, and will until its TTL runs out", new Guid("318cce6c-294a-d561-1f9a-9e4765493497"), "A recursive resolver, such as the office DNS server or an ISP (internet service provider)'s, looks a name up once and then keeps the answer for the TTL on the record, which was 86400 seconds, or 24 hours. Anyone who asked shortly before 2 PM cached the old address for up to a day. Resolvers that hadn't cached it, or whose copy has expired, fetch the new one. That is all propagation is. Nothing gets pushed out, and each cache expires at its own time.", 2, "The authoritative server returns the new address. The office DNS server still returns the old one. What is happening?", new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85") },
                    { new Guid("d14790d7-0a60-4d52-3154-f93788d4b2d1"), "Turn on aging for the zone and scavenging on one DNS server", new Guid("cc7caa36-7056-09f1-69c8-db5ef5a6a614"), "Every dynamic record carries a timestamp that its owner refreshes. Aging and scavenging uses it to delete records nobody has refreshed. With the default intervals, a record's timestamp can't be refreshed for the first 7 days, the no-refresh interval, and then it has 7 more days, the refresh interval, to be refreshed before it becomes eligible for deletion. It must be turned on in two places: aging on the zone, and scavenging on at least one DNS server. Records created by hand are static, have no timestamp, and are never removed. Check the timestamps before the first run, because a record that should have been static but was registered dynamically will be deleted too.", 5, "How do you stop stale records building up across the zone?", new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80") },
                    { new Guid("d2bd670e-c54a-4703-85f1-5598ab702d6b"), "Run Resolve-DnsName www.corp.example and read the TTL in the answer", new Guid("f4bb85d2-b1ae-d4a6-55dc-9f9f0b4267b9"), "An authoritative server always reports the full TTL. A resolver answering from its cache reports the time remaining, so the number falls each time you ask. Resolve-DnsName prints it in the TTL column, and dig and nslookup -debug show it too. If it reads 79200, the office resolver will hold the old address for another 22 hours unless its cache is cleared.", 3, "How can you tell how much longer the office resolver will keep the old address?", new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85") },
                    { new Guid("dd1c8efc-4978-0b13-3576-aac8cf8b34ba"), "Add corp.example as the DNS suffix in the VPN profile, so that every client receives it", new Guid("03f4f533-8963-4481-9d90-5cec65e8f3ed"), "Fix it at the source of the setting. With corp.example as the VPN connection's DNS suffix, each laptop appends it while connected, and files01 becomes files01.corp.example before the query is sent. On the built-in Windows VPN client that is Set-VpnConnection -DnsSuffix, and other clients have the same setting in their profile or on the VPN server. One change reaches every user, and it goes away when the VPN disconnects. Check with ipconfig /all and by opening \\\\files01.", 5, "What is the right fix?", new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703") },
                    { new Guid("eaec9af4-316b-761c-14f2-69561c41f9d1"), "A stale A (address) record that still holds the address the PC had before the reimage", new Guid("a8a19a66-fe51-abb6-b957-0e70474219e2"), "An A record maps a name to an address. This one was written when PC-ACCT-07 held 10.0.5.42, before the reimage. The PC has since been given 10.0.5.87, and DHCP reused the old address for another laptop, but the record was never updated. A record that outlives the fact it describes is called stale. Windows PCs normally correct their own record whenever their address changes, so the next question is why this one didn't.", 2, "DNS returns 10.0.5.42 for PC-ACCT-07. The PC actually has 10.0.5.87, and DHCP shows 10.0.5.42 leased to a sales laptop. What is this?", new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80") },
                    { new Guid("eef64e0d-a499-2f6d-f445-5b885d0a321a"), "The old VPN profile set corp.example as the adapter's DNS suffix, and the new one doesn't", new Guid("4669e98f-84b1-4900-d4ec-7ddec3be98ad"), "A laptop that isn't joined to the domain has no primary suffix, so the only source for corp.example was the VPN connection. The old client's profile supplied it as the connection-specific suffix, and the new profile left it out. Now the laptop tries files01.home, gets no answer, and falls back to LLMNR (Link-Local Multicast Name Resolution) and NetBIOS broadcasts. Those only reach the user's home network and never cross the VPN, so the lookup fails.", 4, "The primary suffix is blank, the VPN adapter has no connection-specific suffix, and the search list holds only 'home', from the user's router. Why did short names work with the old client?", new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703") }
                });

            migrationBuilder.InsertData(
                table: "Distractors",
                columns: new[] { "Id", "Explanation", "SequentialStepId", "Text" },
                values: new object[,]
                {
                    { new Guid("08747662-0148-e619-c8fc-16bb36b1d9a3"), "That address is leased to the sales laptop, so the two would conflict.", new Guid("5d2fca48-64b3-703d-c447-2f1ac4ef6fd6"), "Give PC-ACCT-07 a static address of 10.0.5.42" },
                    { new Guid("101fc3e5-718b-cd35-c27d-f311cb3c2a5d"), "Ping has to resolve the name first through the same failing server, so it just fails the same way.", new Guid("567a9711-2995-bc9b-f46c-19a1f539fd82"), "Ping www.microsoft.com" },
                    { new Guid("125a2de1-0a3d-6faa-3919-8298bc7ca79f"), "Addresses would be reused sooner, so stale records would point at the wrong device more often.", new Guid("d14790d7-0a60-4d52-3154-f93788d4b2d1"), "Shorten the DHCP lease to one hour" },
                    { new Guid("127529d0-14e5-f76d-ecfe-7c6294595e6a"), "DNS has no separate short-name records. Every record is stored under its full name.", new Guid("6edca510-7231-eb8e-485a-4f2ba429ab56"), "The file server registers its short name only with the DNS servers in the office, so remote clients can never see it" },
                    { new Guid("13f7d282-d106-5e45-76b8-4fb9fbc6fe2a"), "Every PC in the office fails the same way, so one browser's settings aren't the cause.", new Guid("75a0ff91-ea0d-ef72-7c1a-499eef3c0860"), "Reset the browser's settings and clear its cache on the PC" },
                    { new Guid("17913f8a-7913-7d12-e377-d793edb9fad7"), "The connection never reaches PC-ACCT-07, so nothing on that PC affects it.", new Guid("9afc475b-4e56-12d7-8f35-801d415db20b"), "Restart the Remote Desktop Services service on PC-ACCT-07 and ask the help desk to connect by name once more" },
                    { new Guid("18630f3d-7182-5c3e-4835-33a97055cb2b"), "It is slow and error-prone, and it leaves up to a month of stale records between passes.", new Guid("d14790d7-0a60-4d52-3154-f93788d4b2d1"), "Have a technician delete old records by hand each month, checking each one against the DHCP lease list" },
                    { new Guid("1a60cda5-ce6a-dc4d-1f2d-ceffd8a8c994"), "A flush removes cached answers. It can't supply an answer that was never resolved.", new Guid("ab953f6a-d292-5339-39a3-4846d269351a"), "Run ipconfig /flushdns and ask the user to try files01 again" },
                    { new Guid("1bd5671a-813a-b803-4992-e2d8cdab9f9d"), "Traffic already reaches 8.8.8.8 through the firewall, so a restart interrupts everyone and tests nothing.", new Guid("75a0ff91-ea0d-ef72-7c1a-499eef3c0860"), "Restart the firewall, since the ISP (internet service provider) change was made on it last night and nothing has loaded since" },
                    { new Guid("25368ff3-773d-164f-1e4a-5622f4fae01d"), "nslookup doesn't read the PC's cache. It asks the server directly, and the server is what timed out.", new Guid("12869525-01a1-e3d2-9a7e-8d2f62b18c2e"), "Every PC has a corrupted local DNS cache, and each one needs ipconfig /flushdns before external names will resolve again" },
                    { new Guid("264f97a6-d7d1-07e3-743d-a725cc2aa10b"), "Round robin needs several records under one name. Here there is one record, and it is wrong.", new Guid("eaec9af4-316b-761c-14f2-69561c41f9d1"), "Round robin DNS, which rotates between the two addresses so that the load is shared across both of the computers" },
                    { new Guid("26cf3ac2-a804-3e66-e26b-fad5141424b3"), "Clients already receive 10.0.0.10 as their DNS server, and that is correct.", new Guid("12472fe7-e3ca-b1cf-9e83-2aeca7b364c0"), "The DHCP scope options" },
                    { new Guid("37343d63-2d00-edfb-1b37-73d083d31f32"), "It hides the fault on a few PCs and leaves the wrong record for everyone else.", new Guid("5d2fca48-64b3-703d-c447-2f1ac4ef6fd6"), "Add the correct address to the hosts file on the help desk PCs" },
                    { new Guid("389a49e6-cec6-1163-2401-a1f469967e1c"), "Each PC has its own address and both work. Only the DNS record is wrong.", new Guid("eaec9af4-316b-761c-14f2-69561c41f9d1"), "An IP (Internet Protocol) address conflict between the two computers" },
                    { new Guid("3f55e1d6-e0b7-e150-b32f-64c65b2eb89b"), "A 24-hour TTL outlasts the night, so daytime users would still get the old address.", new Guid("73b1d146-0486-fabd-60aa-2a7f6443355f"), "Make the change at night" },
                    { new Guid("42293dec-bbf6-6031-14fe-2f1706c10f52"), "Several names can point at one address. The laptop's own record doesn't block this one.", new Guid("5b6ca569-8d04-1904-b86f-d7411c8cae44"), "The sales laptop registered the address first, and DNS allows only one name for each IP address in a zone" },
                    { new Guid("4ad7c7a9-b9dd-38e0-0741-46649e6df26a"), "A VPN connection doesn't join a PC to a domain. That is a one-time change with its own account.", new Guid("eef64e0d-a499-2f6d-f445-5b885d0a321a"), "The old client joined the laptop to the domain each time it connected" },
                    { new Guid("4d39f973-87b1-f3f2-e82a-73f85a44aee0"), "It would only suit users whose router hands out that suffix, and it leaves duplicate records to maintain.", new Guid("dd1c8efc-4978-0b13-3576-aac8cf8b34ba"), "Create a zone named home on the company DNS server with copies of the records" },
                    { new Guid("4dbd9941-3b53-09e4-2524-4002311a125f"), "A reply only shows that something answers at that address. It doesn't say which machine it is.", new Guid("9afc475b-4e56-12d7-8f35-801d415db20b"), "Ping PC-ACCT-07 again" },
                    { new Guid("4e2cf942-db1f-0d9d-3168-bb008cb582e8"), "Both leases came from the real server and appear in its lease list.", new Guid("eaec9af4-316b-761c-14f2-69561c41f9d1"), "A rogue DHCP server" },
                    { new Guid("4e4689e5-155c-b35c-2602-f699c2d7e124"), "The hosts file maps names to addresses. It holds no suffix settings.", new Guid("8a984beb-c5b1-6ca1-70ba-6d9c12fc8f59"), "In the hosts file" },
                    { new Guid("4ee48f93-ee37-0933-a5a7-affbe1aba890"), "The SOA (start of authority) record describes the zone. Zones don't tell clients which suffixes to add.", new Guid("8a984beb-c5b1-6ca1-70ba-6d9c12fc8f59"), "In the output of nslookup -type=SOA corp.example, which lists the suffixes that the zone tells clients to use" },
                    { new Guid("58e60e90-5aad-7329-d043-357200655d6f"), "The server is answering for corp.example, so the zone is loaded and healthy.", new Guid("12472fe7-e3ca-b1cf-9e83-2aeca7b364c0"), "The corp.example zone's SOA (start of authority) record and its serial number, in case the zone has stopped loading on this server" },
                    { new Guid("5d98fceb-c41d-876c-6b6d-4c36e154a035"), "Resolvers don't check whether a server is alive. Customers with the cached address would just get an error.", new Guid("67b42063-70d5-8463-b9db-67d929a6b203"), "Shut the old server down so that resolvers are forced to look up the new address" },
                    { new Guid("671dd343-99a3-b530-da53-c363a04f4676"), "Every visit would then need a fresh lookup, which slows the site, and some resolvers enforce a minimum anyway.", new Guid("73b1d146-0486-fabd-60aa-2a7f6443355f"), "Set the TTL to 0 permanently, so that no resolver anywhere ever caches the record and every visitor always gets the current address" },
                    { new Guid("681e5c6f-ac5c-efab-2c70-20e9e5357d59"), "That is the TTL of an IP (Internet Protocol) packet, a hop limit. The DNS TTL is a cache lifetime in seconds, and the two only share a name.", new Guid("d2bd670e-c54a-4703-85f1-5598ab702d6b"), "Run tracert to the old server and count the hops, since each router on the way takes one off the TTL" },
                    { new Guid("6bb98134-a8e8-b28b-990f-6911338c13f9"), "The lease would hand back the same server, 10.0.0.10, which is the correct one for this network.", new Guid("567a9711-2995-bc9b-f46c-19a1f539fd82"), "Run ipconfig /release and ipconfig /renew on the PC to get a fresh lease and a new set of DNS servers from DHCP (Dynamic Host Configuration Protocol)" },
                    { new Guid("6d30111d-49b8-4736-6638-443b9fa58899"), "A hosts file only affects lookups that machine makes for itself, not the answers it gives to clients.", new Guid("12472fe7-e3ca-b1cf-9e83-2aeca7b364c0"), "The hosts file on the server, for entries that override public names" },
                    { new Guid("7493f3cd-c0b9-8524-9cb2-532d707a4890"), "A stopped service couldn't have answered for intranet.corp.example.", new Guid("12869525-01a1-e3d2-9a7e-8d2f62b18c2e"), "The DNS Server service on 10.0.0.10 has stopped" },
                    { new Guid("7664e3dd-416c-13e3-d1e5-83710c21cd5c"), "Saving it again changes nothing if it is already correct, and resolvers aren't notified either way.", new Guid("a8370a9a-c31f-427c-4d5c-04d2a8ba31ec"), "Change the A record again to the same value so that it is published a second time" },
                    { new Guid("779e3d69-145e-31a1-ce16-9618cbb0a371"), "Nothing suggests a hosts file was in use, and the missing suffix explains the failure completely.", new Guid("eef64e0d-a499-2f6d-f445-5b885d0a321a"), "The old client copied the company's hosts file to each laptop when it connected, and the new client has stopped doing that" },
                    { new Guid("79a05ffb-5a08-7556-b505-10ae477b2f81"), "No such mechanism exists. Resolvers fetch a record again only when their copy expires.", new Guid("73b1d146-0486-fabd-60aa-2a7f6443355f"), "Ask the DNS provider to push the new record out to ISPs' resolvers faster" },
                    { new Guid("7e407c6c-73f3-8918-98ea-41bbae09ab43"), "The authoritative server answers with the new address, and a damaged record couldn't.", new Guid("cdaec7f1-7b9e-ed86-a3e1-0d0890937c06"), "The record is corrupted" },
                    { new Guid("84da22ab-cff2-4d5d-3217-c42813c1f94a"), "Every external name fails, not one, and a fault that broad points to your own resolver.", new Guid("12869525-01a1-e3d2-9a7e-8d2f62b18c2e"), "The microsoft.com zone is down" },
                    { new Guid("8c29c84c-2c99-810d-be3f-6288a9d54484"), "There's no such waiting period. A PC registers at startup and whenever its address changes.", new Guid("5b6ca569-8d04-1904-b86f-d7411c8cae44"), "Reimaged PCs wait 30 days to register" },
                    { new Guid("8c7809d9-7179-de5c-059c-4af6459c7e2f"), "Customers who reach the new server get a working site, so the server isn't the problem.", new Guid("a8370a9a-c31f-427c-4d5c-04d2a8ba31ec"), "Reboot the new web server" },
                    { new Guid("8cb342e5-8dea-27ec-24cb-e3e1ca5f39e1"), "A restart doesn't change who the server forwards to, and those resolvers won't answer a non-customer.", new Guid("2d6f646c-6cdb-6c21-d3d7-1d52dcb32cba"), "Restart the DNS Server service every morning with a scheduled task until the old ISP's resolvers start answering the office again" },
                    { new Guid("8dfb9ad3-1beb-d41a-ba09-b91d7cd9fe59"), "That shows what different caches hold. It can't tell you whether the published record is right.", new Guid("a8370a9a-c31f-427c-4d5c-04d2a8ba31ec"), "Ping www.corp.example from several office PCs and from a few phones on mobile data, and compare the addresses that reply" },
                    { new Guid("903839c9-7466-bcf2-c008-2857adb99a85"), "The VPN is connected and full names open through it, so the tunnel is already known to work.", new Guid("ab953f6a-d292-5339-39a3-4846d269351a"), "Ping the VPN gateway's public address" },
                    { new Guid("919a3819-9529-fbb1-2fe0-a68eb3ba7d83"), "The full name is that record, and it resolved.", new Guid("6edca510-7231-eb8e-485a-4f2ba429ab56"), "The files01 record is missing from the corp.example zone" },
                    { new Guid("924e21da-24ce-d2f1-8e80-b89ab049f660"), "A file date on a web server says nothing about what a resolver has cached.", new Guid("d2bd670e-c54a-4703-85f1-5598ab702d6b"), "Look at the Last Modified date on the old site's home page" },
                    { new Guid("93b6e539-1338-8d6b-3029-add3494767b6"), "The provider's own authoritative server is already returning the new address.", new Guid("cdaec7f1-7b9e-ed86-a3e1-0d0890937c06"), "The DNS provider hasn't finished saving the change" },
                    { new Guid("944b9599-5c94-c555-aa88-aebcdd11c63a"), "Any device could then overwrite any record, including a server's, which opens the zone to spoofing.", new Guid("5d2fca48-64b3-703d-c447-2f1ac4ef6fd6"), "Set the zone to accept nonsecure and secure updates, so that any device is allowed to overwrite the record" },
                    { new Guid("95a3d0fa-9dd2-03fd-bc80-f65804bb295c"), "The log shows when the user connected. Suffixes are a setting on the client.", new Guid("8a984beb-c5b1-6ca1-70ba-6d9c12fc8f59"), "In the VPN server's connection log for this user" },
                    { new Guid("9743791d-e2b7-cff2-0d93-b605f19904c1"), "The wrong address comes from the DNS server, so the PC would just fetch the same answer again.", new Guid("9afc475b-4e56-12d7-8f35-801d415db20b"), "Run ipconfig /flushdns on the help desk PC and try again" },
                    { new Guid("9b433e39-51f5-36ef-2daa-f24ad9a746da"), "Mapped drives, shortcuts and scripts already use the short names, and they would all stay broken.", new Guid("dd1c8efc-4978-0b13-3576-aac8cf8b34ba"), "Tell staff to type the full name from now on" },
                    { new Guid("9c251448-e7e6-5240-2d4a-1cd5358cea1c"), "A hosts entry fixes one name on one PC, and it goes wrong when the site's address changes.", new Guid("2d6f646c-6cdb-6c21-d3d7-1d52dcb32cba"), "Add www.microsoft.com to each PC's hosts file" },
                    { new Guid("ae79f59e-f3e4-3543-143e-0ad1d984b9c1"), "A VPN carries packets. It has no view of how a name was typed before the query was built.", new Guid("6edca510-7231-eb8e-485a-4f2ba429ab56"), "The VPN blocks short names" },
                    { new Guid("af0e11a6-4d01-3553-ac9f-4ca1c9b9cf32"), "A disabled service wouldn't attempt a registration, and event 8015 records an attempt that failed.", new Guid("5b6ca569-8d04-1904-b86f-d7411c8cae44"), "The DNS Client service is disabled on the PC" },
                    { new Guid("b978e4d2-6d2a-c215-70a0-652aa4b4335f"), "That is when the domain registration lapses. It has nothing to do with cached records.", new Guid("d2bd670e-c54a-4703-85f1-5598ab702d6b"), "Check the domain's expiry date in the registrar's WHOIS record" },
                    { new Guid("ba44955f-1232-d768-479c-5f7cb6aef74b"), "It gives up DHCP for the whole site, and retired PCs would still leave records behind.", new Guid("d14790d7-0a60-4d52-3154-f93788d4b2d1"), "Give every PC a static address so that its record never changes" },
                    { new Guid("bc08be54-6b9b-4c43-51c4-b9210b89d3c0"), "Resolvers that hold the old answer won't ask again until the old 24-hour TTL expires, so they don't see the new value in time.", new Guid("67b42063-70d5-8463-b9db-67d929a6b203"), "Lower the record's TTL to 60 seconds now" },
                    { new Guid("c2f58df4-2279-8063-134f-7a0057596a29"), "A public resolver can't answer for corp.example, so the intranet, file shares and domain sign-in would break.", new Guid("2d6f646c-6cdb-6c21-d3d7-1d52dcb32cba"), "Change the DHCP scope so that PCs use 8.8.8.8 as their DNS server" },
                    { new Guid("d439b087-a1de-1ff2-0587-ae678758305f"), "Nothing notifies resolvers, and anyone who looks the name up during the gap caches a failure instead.", new Guid("67b42063-70d5-8463-b9db-67d929a6b203"), "Delete the A record, wait a few minutes, and create it again so that every resolver on the internet notices the change and refreshes" },
                    { new Guid("d885bc9d-83d5-6613-3d7a-98d92be2bfdb"), "Web hosts don't publish DNS answers. Only the domain's authoritative name servers do.", new Guid("cdaec7f1-7b9e-ed86-a3e1-0d0890937c06"), "The old web host is still advertising the old address to resolvers, which overrides the new record until the hosting account is closed" },
                    { new Guid("e465f69f-4c11-8886-f36a-dbe61976a82e"), "Repeating the same query to the same server can't show where the fault lies.", new Guid("567a9711-2995-bc9b-f46c-19a1f539fd82"), "Run nslookup www.microsoft.com again a few minutes later" },
                    { new Guid("f723ef64-1489-c146-b2b7-6bde3ad210e5"), "The ping already proved that path works. Another test of the same path adds nothing.", new Guid("75a0ff91-ea0d-ef72-7c1a-499eef3c0860"), "Run tracert 8.8.8.8" },
                    { new Guid("f7bc592b-c788-203f-ff44-2a5f3bf4fffb"), "Speed isn't the issue. The full name resolves at once through the current server.", new Guid("eef64e0d-a499-2f6d-f445-5b885d0a321a"), "The old client used a faster DNS server" },
                    { new Guid("f9bc30e1-d888-63f3-0f3d-b07d1c33d623"), "It is a slow test that removes the configuration you need to inspect, and the old client is being retired.", new Guid("ab953f6a-d292-5339-39a3-4846d269351a"), "Uninstall the new VPN client and reinstall the old one on this laptop to see whether the problem follows the client" },
                    { new Guid("fa766293-cd32-1670-e91f-6957f9151247"), "It covers two names by hand on every laptop, and it breaks without warning when an address changes.", new Guid("dd1c8efc-4978-0b13-3576-aac8cf8b34ba"), "Add files01 and intranet to the hosts file on every remote laptop, and update each file whenever a server's address changes" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("08747662-0148-e619-c8fc-16bb36b1d9a3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("101fc3e5-718b-cd35-c27d-f311cb3c2a5d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("125a2de1-0a3d-6faa-3919-8298bc7ca79f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("127529d0-14e5-f76d-ecfe-7c6294595e6a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("13f7d282-d106-5e45-76b8-4fb9fbc6fe2a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("17913f8a-7913-7d12-e377-d793edb9fad7"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("18630f3d-7182-5c3e-4835-33a97055cb2b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1a60cda5-ce6a-dc4d-1f2d-ceffd8a8c994"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1bd5671a-813a-b803-4992-e2d8cdab9f9d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("25368ff3-773d-164f-1e4a-5622f4fae01d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("264f97a6-d7d1-07e3-743d-a725cc2aa10b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("26cf3ac2-a804-3e66-e26b-fad5141424b3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("37343d63-2d00-edfb-1b37-73d083d31f32"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("389a49e6-cec6-1163-2401-a1f469967e1c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3f55e1d6-e0b7-e150-b32f-64c65b2eb89b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("42293dec-bbf6-6031-14fe-2f1706c10f52"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4ad7c7a9-b9dd-38e0-0741-46649e6df26a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4d39f973-87b1-f3f2-e82a-73f85a44aee0"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4dbd9941-3b53-09e4-2524-4002311a125f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4e2cf942-db1f-0d9d-3168-bb008cb582e8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4e4689e5-155c-b35c-2602-f699c2d7e124"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4ee48f93-ee37-0933-a5a7-affbe1aba890"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("58e60e90-5aad-7329-d043-357200655d6f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5d98fceb-c41d-876c-6b6d-4c36e154a035"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("671dd343-99a3-b530-da53-c363a04f4676"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("681e5c6f-ac5c-efab-2c70-20e9e5357d59"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6bb98134-a8e8-b28b-990f-6911338c13f9"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6d30111d-49b8-4736-6638-443b9fa58899"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7493f3cd-c0b9-8524-9cb2-532d707a4890"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7664e3dd-416c-13e3-d1e5-83710c21cd5c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("779e3d69-145e-31a1-ce16-9618cbb0a371"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("79a05ffb-5a08-7556-b505-10ae477b2f81"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7e407c6c-73f3-8918-98ea-41bbae09ab43"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("84da22ab-cff2-4d5d-3217-c42813c1f94a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8c29c84c-2c99-810d-be3f-6288a9d54484"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8c7809d9-7179-de5c-059c-4af6459c7e2f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8cb342e5-8dea-27ec-24cb-e3e1ca5f39e1"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8dfb9ad3-1beb-d41a-ba09-b91d7cd9fe59"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("903839c9-7466-bcf2-c008-2857adb99a85"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("919a3819-9529-fbb1-2fe0-a68eb3ba7d83"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("924e21da-24ce-d2f1-8e80-b89ab049f660"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("93b6e539-1338-8d6b-3029-add3494767b6"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("944b9599-5c94-c555-aa88-aebcdd11c63a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("95a3d0fa-9dd2-03fd-bc80-f65804bb295c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9743791d-e2b7-cff2-0d93-b605f19904c1"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9b433e39-51f5-36ef-2daa-f24ad9a746da"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9c251448-e7e6-5240-2d4a-1cd5358cea1c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ae79f59e-f3e4-3543-143e-0ad1d984b9c1"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("af0e11a6-4d01-3553-ac9f-4ca1c9b9cf32"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b978e4d2-6d2a-c215-70a0-652aa4b4335f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ba44955f-1232-d768-479c-5f7cb6aef74b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("bc08be54-6b9b-4c43-51c4-b9210b89d3c0"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c2f58df4-2279-8063-134f-7a0057596a29"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d439b087-a1de-1ff2-0587-ae678758305f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d885bc9d-83d5-6613-3d7a-98d92be2bfdb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e465f69f-4c11-8886-f36a-dbe61976a82e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f723ef64-1489-c146-b2b7-6bde3ad210e5"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f7bc592b-c788-203f-ff44-2a5f3bf4fffb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f9bc30e1-d888-63f3-0f3d-b07d1c33d623"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fa766293-cd32-1670-e91f-6957f9151247"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("12472fe7-e3ca-b1cf-9e83-2aeca7b364c0"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("12869525-01a1-e3d2-9a7e-8d2f62b18c2e"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("2d6f646c-6cdb-6c21-d3d7-1d52dcb32cba"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("567a9711-2995-bc9b-f46c-19a1f539fd82"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5b6ca569-8d04-1904-b86f-d7411c8cae44"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("5d2fca48-64b3-703d-c447-2f1ac4ef6fd6"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("67b42063-70d5-8463-b9db-67d929a6b203"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6edca510-7231-eb8e-485a-4f2ba429ab56"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("73b1d146-0486-fabd-60aa-2a7f6443355f"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("75a0ff91-ea0d-ef72-7c1a-499eef3c0860"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a984beb-c5b1-6ca1-70ba-6d9c12fc8f59"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9afc475b-4e56-12d7-8f35-801d415db20b"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a8370a9a-c31f-427c-4d5c-04d2a8ba31ec"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ab953f6a-d292-5339-39a3-4846d269351a"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("cdaec7f1-7b9e-ed86-a3e1-0d0890937c06"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d14790d7-0a60-4d52-3154-f93788d4b2d1"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("d2bd670e-c54a-4703-85f1-5598ab702d6b"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("dd1c8efc-4978-0b13-3576-aac8cf8b34ba"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("eaec9af4-316b-761c-14f2-69561c41f9d1"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("eef64e0d-a499-2f6d-f445-5b885d0a321a"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("294aa80e-5e75-fe86-3801-0db8b4486a80"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("aec21c3b-b5e8-3139-34e5-a9b02dee3703"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("b2cf2e5f-01d8-da93-07ae-78133f048072"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("c7e674d9-bc19-8426-36d2-f0f4dd374d85"));
        }
    }
}
