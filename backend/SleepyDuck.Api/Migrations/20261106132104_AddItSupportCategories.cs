using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddItSupportCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScenarioCards",
                columns: new[] { "Id", "Category", "ProblemStatement", "TechnicalExplanation", "Title" },
                values: new object[,]
                {
                    { new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47"), "VoipSupport", "Every desk phone at a branch office can place and answer calls through the company's hosted PBX, but on all calls the staff hear nothing while the people they call hear them clearly. Phones show as registered, and the problem began after the branch firewall was replaced.", "SIP sets up a call, and RTP carries the audio as a separate UDP stream, so a call can connect perfectly with audio failing in one direction. Each side uses the SDP body in the SIP messages to announce the IP address and port where it wants to receive RTP. When a phone behind NAT advertises its private address, the far end sends audio to an address it can't route to. This is the classic one-way audio pattern, and it fits here because the new firewall changed how NAT was handled. Fix it with proper NAT traversal, such as STUN, or an SBC or PBX that uses symmetric RTP. Also disable SIP ALG if the firewall's rewriting is making things worse, and make sure the RTP UDP port range is allowed. Compare captures from both sides of the firewall, because they show whether a stream was never sent or was lost on the way.", "One-way audio at a branch office" },
                    { new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76"), "VoipSupport", "Calls to a remote worker's extension fail after about 32 seconds, and the caller's phone shows 408 Request Timeout. The phone sits behind a home router and shows as registered. Oddly, it's reachable for a minute or two right after it's rebooted, and calls made from that phone always work.", "A SIP 408 means a request got no final response within the allowed time. For an INVITE over UDP, Timer B expires at 64 times T1, which is 32 seconds with the default 500 ms T1, and that matches this ticket. Retransmissions with no provisional response show the INVITE isn't reaching the phone, or the phone's reply isn't getting back. Outbound calls work because the phone opens its own path through the router. Inbound calls depend on a NAT or firewall UDP mapping staying open. When the mapping expires between registrations, the PBX's registration is still valid, but its INVITE is dropped at the router. Keep-alives sent more often than the router's UDP timeout hold the mapping open. Avoid changing SIP timers, because a longer wait just delays the same failure.", "SIP 408 Request Timeout on a remote phone" },
                    { new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538"), "NetworkTroubleshooting", "Users at a branch office report choppy video calls and stalls in a cloud app at 203.0.113.50, starting this morning. Everything on the local file server works normally. You're on a Linux machine on the branch LAN.", "Ping is the quickest way to scope a problem: test the default gateway first, then an external target. MTR then combines ping and traceroute by sending a stream of probes to every hop. The rule for reading it is that loss only matters if it continues through to the final destination. A router in the middle showing loss when the hops after it don't is usually just rate limiting the ICMP or TTL-expired replies it generates, and it's forwarding traffic fine. Real loss starts at one hop and carries through to the end, which points to that hop or the link leading to it. Paths are often asymmetric, so run MTR from the far side too if you can, and check interface errors and utilization on your own equipment before escalating. Give the ISP timestamped MTR reports from both directions.", "Diagnosing packet loss with ping and MTR" },
                    { new Guid("95e42524-b675-8aab-3bae-f938012f09ed"), "MacNetworkIssues", "Your team moved intranet.corp.example to a new server this morning and updated DNS. Windows laptops reach the new site, but a MacBook still loads the old server. The Mac has a normal internet connection, and browsing elsewhere works fine.", "macOS caches DNS results in the mDNSResponder daemon, and most apps resolve names through that system resolver. Tools such as dig and nslookup query DNS servers directly and skip the system resolver, while dscacheutil -q host follows the path that apps use. That makes comparing them a good test: if dig shows the new address and dscacheutil shows the old one, the fault is on the Mac rather than in DNS. Clear it with sudo dscacheutil -flushcache followed by sudo killall -HUP mDNSResponder. The HUP signal tells the daemon to drop its cache without stopping it. If the old answer survives a flush, look at what the system resolver checks besides DNS, such as an entry in /etc/hosts, or a scoped resolver from a VPN, which scutil --dns will list.", "Stale DNS cache on macOS" },
                    { new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29"), "MacNetworkIssues", "MacBooks on the WPA2-Enterprise SSID CorpSecure started dropping off Wi-Fi about 8 hours after connecting, and they fail to rejoin. It started the day after the RADIUS server's certificate was renewed. Windows laptops are fine. The Macs connect using a configuration profile from your MDM.", "802.1X authenticates a device before it's given network access. With PEAP or EAP-TLS, the client validates the RADIUS server's certificate during the TLS handshake. On macOS, the eapolclient process handles this, and an MDM profile normally defines which certificates and server names to trust. Macs stay connected on an existing session, then fail when reauthentication happens, which explains drops long after the certificate change. Here the 8-hour pattern matches the session timeout. Windows machines were fine because they trust the new chain another way. The right fix is to push an updated profile containing the new CA chain and trusted server names. Telling users to click Trust or turning off validation trains people to accept a fake access point, which is how credentials get stolen. On Windows NPS, event 6272 means access was granted and 6273 means it was denied.", "802.1X Wi-Fi drops on MacBooks" },
                    { new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c"), "NetworkTroubleshooting", "At 9 AM, new devices on the staff and guest VLAN 20 (192.168.20.0/24) can't get online and show 169.254.x.x addresses. Devices that were already connected still work. The VLAN uses a Windows DHCP server, and a conference has been running at the office all week.", "A 169.254.x.x address is an APIPA link-local address, which a client assigns itself when no DHCP server offers a lease. Devices that already hold a lease keep working, which is the classic sign of exhaustion rather than a server outage. A /24 has only 254 usable addresses, and a Windows DHCP scope defaults to an 8-day lease, so visitors' phones keep addresses long after leaving. Phones using private (randomized) MAC addresses can make this worse, since one device can look like several clients. Shorten the lease on busy guest networks, and reclaim stale leases. Changing the duration only affects new leases and renewals, so it needs some time or lease cleanup. Turn on conflict detection while reclaiming, since a device might still be using a removed address. Then fix capacity properly by widening the subnet or moving guests to their own VLAN.", "Resolving DHCP scope exhaustion" }
                });

            migrationBuilder.InsertData(
                table: "SequentialSteps",
                columns: new[] { "Id", "CorrectAction", "CorrectOptionId", "Explanation", "Order", "Prompt", "ScenarioCardId" },
                values: new object[,]
                {
                    { new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"), "DNS is correct, and the Mac's system resolver is serving a stale answer", new Guid("4501088b-f981-0a3b-1f4a-9bba82d87024"), "The DNS server gave the right answer directly. Only the local path that apps use is still wrong, so the fault is on the Mac.", 2, "dig returns the new IP, and dscacheutil returns the old one. What does that show?", new Guid("95e42524-b675-8aab-3bae-f938012f09ed") },
                    { new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"), "sudo dscacheutil -flushcache; sudo killall -HUP mDNSResponder", new Guid("282a33bb-4946-160f-ca3d-71d6cb48d26d"), "This flushes the directory service cache, then signals mDNSResponder to drop its cached DNS results.", 3, "Which command clears the DNS cache on macOS?", new Guid("95e42524-b675-8aab-3bae-f938012f09ed") },
                    { new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"), "Certificates are only checked when authenticating, and reauthentication happens at session timeout", new Guid("7bf9073f-2bfb-84d2-c6e2-5eb6efc7bc15"), "A connected session isn't re-validated continuously. The new certificate only comes into play at the next authentication, which fits the 8-hour pattern.", 3, "Why did the Macs only drop about 8 hours later, and not the moment the certificate changed?", new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29") },
                    { new Guid("22105984-345f-f942-460c-ef2a7966ae39"), "Open an ISP ticket with timestamped MTR reports from both directions", new Guid("95df5a64-6424-2817-fedb-50c917a16a57"), "This gives the ISP proof that the loss starts at its handoff and that your side has been ruled out, which shortens the investigation.", 6, "The firewall's interface is clean and under 40% utilized. What is the right escalation?", new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538") },
                    { new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"), "The INVITE isn't reaching the phone, or its replies aren't returning, until Timer B expires", new Guid("240a7c08-788f-1bf9-8519-7deadd933afd"), "Retransmission means nothing came back. A phone that received the INVITE would answer with a provisional response, and 32 seconds matches Timer B.", 3, "The ladder shows the PBX resending the INVITE to the phone's registered public address at 0.5, 1, 2 and 4 seconds, with no 100 Trying or 180 Ringing. What does this mean?", new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76") },
                    { new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"), "Confirm the profile is installed, then verify NPS event 6272 through a full session timeout", new Guid("1d0c9262-1082-0158-5548-7b23d93fa67b"), "The fault appeared at reauthentication, so a successful grant after a full 8-hour session is the evidence that counts.", 5, "How do you confirm the fix has worked?", new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29") },
                    { new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"), "The VLAN 20 scope statistics on the DHCP server, with Get-DhcpServerv4ScopeStatistics", new Guid("4e773647-2d8b-f2ec-f765-b5490084fa43"), "Existing clients working suggests the server is reachable. Scope statistics show right away whether the pool has run out of addresses.", 2, "Existing devices work, but new ones don't. What do you check first?", new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c") },
                    { new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"), "Match the drop times in the Mac's eapolclient logs against the RADIUS server's logs", new Guid("01b89867-bffe-fe68-deca-0a7668e9b607"), "Authentication has two sides. The Mac shows what the client saw, and RADIUS shows why the request was accepted or rejected.", 1, "Where do you gather evidence first?", new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29") },
                    { new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"), "Ignore hop 5, since loss that doesn't carry on is likely ICMP rate limiting", new Guid("17292a51-1060-ae78-a723-cc526a39a062"), "If hop 5 were really dropping traffic, later hops would show loss too. It's just giving low priority to replies aimed at itself.", 3, "MTR shows hop 5 at 40% loss, but hops 6 to 9, including the destination, show 0%. What do you conclude?", new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538") },
                    { new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"), "Capture the call's SIP signaling on the PBX with sngrep or tcpdump and read the ladder", new Guid("41b26ba6-08b4-6bc7-dacb-9146806599bd"), "A SIP ladder shows each request and response in order, so you can see exactly what the PBX sent and what it got back.", 2, "How do you see where the call is failing?", new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76") },
                    { new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"), "The phone is advertising a private IP, so the far end sends RTP to an unroutable address", new Guid("7774ca8c-5423-119f-2707-0206aa7e33b1"), "The SDP c= line tells the other side where to send audio. A 192.168.x.x address means nothing on the internet, so the return stream never arrives.", 3, "In the capture, the phone's SDP says c=IN IP4 192.168.10.25. RTP from the phone leaves the site, but no RTP comes back. What is the cause?", new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47") },
                    { new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"), "Short-stay devices are holding addresses for 8 days, draining a small /24 scope", new Guid("1c0170cb-513e-4d53-1091-004426c62597"), "Visitors' devices keep their addresses long after they've left, so the pool fills up with leases nobody is really using.", 3, "The scope shows 100% in use. The lease list is full of phones from the conference, with 8-day leases and many private MAC addresses. What is the root cause?", new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c") },
                    { new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"), "SIP signaling is working, so the fault is in the RTP media path", new Guid("b58d8e8f-6c3c-e483-13aa-e372d338fffc"), "A call that rings and connects has completed its SIP exchange. Audio travels in a separate RTP stream that can fail on its own.", 1, "Calls connect normally, but audio only flows one way. What does that tell you?", new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47") },
                    { new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"), "Real loss begins at hop 3 or the link into it, which is your ISP handoff", new Guid("db871cd6-e24f-8833-2ed3-9219e7fd09df"), "Loss that starts at a hop and continues to the destination is genuine. Here that places it at the firewall-to-ISP link or the ISP's gateway.", 4, "A later MTR shows 0% at hops 1 and 2, then 6% at hop 3, and about 6% on every hop through to the destination. Hop 2 is your firewall, and hop 3 is the ISP's gateway. What does this show?", new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538") },
                    { new Guid("827bc225-d24a-2406-a175-96abdf975124"), "Compare dig intranet.corp.example with dscacheutil -q host -a name intranet.corp.example", new Guid("86062839-10b5-0812-331e-54d1d8c1d53c"), "dig asks the DNS server directly, while dscacheutil goes through the macOS resolver that apps use. A difference between them points at the Mac.", 1, "How do you tell whether this is a local cache or a real DNS problem?", new Guid("95e42524-b675-8aab-3bae-f938012f09ed") },
                    { new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"), "Look for an entry in /etc/hosts, then review the resolvers with scutil --dns", new Guid("b464b113-e7e6-a4d2-e95c-73a8f592bbc8"), "The system resolver consults more than the DNS cache. A hosts entry overrides DNS, and a VPN can add a scoped resolver for a domain.", 4, "After the flush, dscacheutil still returns the old IP, even though dig returns the new one. What do you check next?", new Guid("95e42524-b675-8aab-3bae-f938012f09ed") },
                    { new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"), "Take a packet capture during a test call and inspect the SIP and RTP streams in Wireshark", new Guid("bb10e7a4-be03-9eb6-19dd-8f032473b523"), "A capture shows the SDP each side advertised and which RTP streams actually exist. Wireshark's Telephony > VoIP Calls view lays this out for each call.", 2, "How do you get evidence of what is happening to the audio?", new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47") },
                    { new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"), "Wait well past the 60-second UDP timeout, then call in and confirm 100 Trying and 180 Ringing", new Guid("c6bc9318-7b2a-d1ae-0863-97e6910ff952"), "The fault only appears once the mapping has aged out, so the test has to wait for that. A ladder with provisional responses confirms the path stayed open.", 5, "How do you prove the fix works?", new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76") },
                    { new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"), "Shorten the lease to a few hours, reclaim stale leases, and turn on conflict detection", new Guid("aab95c8f-e2b6-566f-942a-7f8ee6734073"), "A shorter lease stops this from recurring, and removing stale leases frees addresses now. Conflict detection guards against an address still being in use.", 4, "What is the best immediate fix?", new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c") },
                    { new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"), "Ping the default gateway, then an external IP, each with about 100 packets", new Guid("98cb03d2-669a-afcd-c99b-1a814f5164aa"), "This quickly separates a LAN problem from a WAN problem. A larger count catches intermittent loss that a few packets would miss.", 1, "Where do you start?", new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538") },
                    { new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"), "Move guests to a separate VLAN, or widen the subnet to a /23 during a change window", new Guid("ece36e9d-109d-d294-3aef-c95cdc28dd96"), "Both add real capacity. A /23 means changing the gateway mask and the scope together, while a guest VLAN also keeps visitors away from staff devices.", 5, "Service is back. How do you prevent it from happening again?", new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c") },
                    { new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"), "Check the WAN interface for errors and saturation, then run MTR from the far side too", new Guid("670546da-0863-2b07-7df3-e514ff0882b8"), "Interface errors, a duplex mismatch or a full link would mean the fault is yours. Return paths can differ, so a reverse MTR gives the ISP the full picture.", 5, "What should you do before contacting the ISP?", new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538") },
                    { new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"), "The client got no DHCP offer and assigned itself a link-local APIPA address", new Guid("a36154ed-0c31-07bd-9a93-9a6356eca927"), "Clients use APIPA when DHCP fails. It's a useful clue, since it means the problem is getting a lease, not routing or DNS.", 1, "What does a 169.254.x.x address tell you?", new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c") },
                    { new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"), "Push an updated MDM profile with the new CA chain and trusted server names", new Guid("b88f1407-4524-a1b3-51c8-dc618ab2adf6"), "This restores proper validation on every managed Mac at once, and users don't need to make any trust decisions.", 4, "What is the correct fix?", new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29") },
                    { new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"), "Configure NAT traversal so SDP carries a reachable address, and allow the RTP port range", new Guid("6fa53860-2e71-390b-3a1e-3451b0438fdb"), "STUN, or an SBC or PBX using symmetric RTP, gets audio back through NAT. The firewall must also pass the provider's RTP UDP range.", 4, "What is the right fix?", new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47") },
                    { new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"), "Enable SIP keep-alives, sent more often than the router's 60-second UDP timeout", new Guid("efc48710-a31a-23ea-c4d5-072974035e7f"), "CRLF or OPTIONS keep-alives refresh the NAT mapping, so inbound INVITEs have an open path. This also explains why rebooting helps for a short time.", 4, "The phone last registered 40 minutes ago, and the home router's UDP timeout is 60 seconds. What is the best fix?", new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76") },
                    { new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"), "mtr -rwc 200 203.0.113.50", new Guid("3f891340-abb7-ff2a-759f-d5af1c642202"), "Report mode with 200 probes per hop gives a statistically useful picture of loss at every hop on the path.", 2, "The gateway shows 0% loss, and the external IP shows 5%. Which command shows where the loss begins?", new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538") },
                    { new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"), "No final response to the request arrived in time, so find where the request stopped", new Guid("6215c595-afce-3fff-4be4-79a792d1e500"), "408 is about time running out. The request got no final answer, so the next job is to find which hop it stopped at.", 1, "What does SIP 408 Request Timeout tell you?", new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76") },
                    { new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"), "Test calls in both directions, confirming two-way audio and two RTP streams in a capture", new Guid("ee34b6e1-caae-d2f7-e96b-7f3608efde07"), "Testing inbound and outbound calls covers both call paths, and the capture proves media flows each way instead of relying on one good impression.", 5, "How do you confirm the fix?", new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47") },
                    { new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"), "The Macs' 802.1X profile doesn't trust the renewed RADIUS certificate chain", new Guid("78ac1865-2d5d-24aa-5182-1f66387d873a"), "An unknown CA alert means the client rejected the server's certificate. The timing matches the renewal, and the Macs are the ones using a fixed trust profile.", 2, "The Mac logs show a TLS failure during EAP, and RADIUS logs an unknown CA alert from the client at each drop. Passwords are correct. What is the cause?", new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29") }
                });

            migrationBuilder.InsertData(
                table: "Distractors",
                columns: new[] { "Id", "Explanation", "SequentialStepId", "Text" },
                values: new object[,]
                {
                    { new Guid("0cc138cb-e592-59b2-9214-cbfddff702ed"), "This removes evidence and may not work on a network managed by a profile.", new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"), "Forget the network on each Mac and then rejoin it manually" },
                    { new Guid("0d366d09-90de-d046-2dbc-7322c6ff8042"), "Rogue DHCP gives clients a wrong address. It doesn't leave them with no offer at all.", new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"), "A rogue DHCP server on the VLAN is handing out bad addresses and taking leases from the real server" },
                    { new Guid("11543112-48f0-da51-1ede-9bba39fce770"), "Longer leases hold addresses for even longer, which makes exhaustion worse.", new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"), "Extend the lease duration to 30 days to stop clients renewing" },
                    { new Guid("11d8b8bb-c299-c006-4cf6-39862c4b0711"), "That's for systemd-resolved on Linux, which macOS doesn't use.", new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"), "sudo resolvectl flush-caches" },
                    { new Guid("1632807d-feb0-35b5-94aa-15964031bec8"), "The cache is already clear, so repeating the flush won't change a persistent source.", new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"), "Run the same flush command again until the result changes" },
                    { new Guid("199754d9-8bee-6eea-2d2c-534802a69c38"), "Bandwidth shortage causes poor quality on both directions, not a clean loss of audio in one.", new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"), "Run an internet speed test from a PC on the branch network" },
                    { new Guid("1b2782a6-3845-b728-6e7f-ad6bc2ead369"), "They were registered during the fault too, so this proves nothing about audio.", new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"), "Confirm that all of the phones still show as registered" },
                    { new Guid("1c186583-f952-1da2-382c-ac7770a62b7f"), "A device needs to authenticate first, so this failure comes before DHCP.", new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"), "The DHCP scope for the staff VLAN has run out of addresses" },
                    { new Guid("1d7f1cc4-08cd-e5b0-536e-b118927e576b"), "The access point only relays EAP. The RADIUS server presents the certificate.", new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"), "The access points stored the old certificate in their memory" },
                    { new Guid("1ef30f6d-e145-2a62-9f7f-663f8beff3dd"), "CDRs show that the call connected and how long it lasted. They don't show which way the media was flowing.", new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"), "Review the call detail records in the hosted PBX portal to see which direction of each call failed" },
                    { new Guid("1f1c546e-edfa-02f9-a196-7079d261c208"), "A single impression can't prove a fault that shows up hours later.", new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"), "Ask one user to say whether Wi-Fi feels more stable today" },
                    { new Guid("2320f06f-c909-4044-bd50-b060889a0c58"), "That hides evidence and wouldn't change any real packet loss.", new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"), "Disable ICMP on the firewall so the MTR reports look cleaner" },
                    { new Guid("23aac207-a651-d480-15c4-15a0e7b2e0af"), "dscacheutil shows the wrong IP before any web traffic happens.", new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"), "The web server is redirecting the Mac back to the old host" },
                    { new Guid("2d5856ff-6304-b4a3-617f-e89b79261b65"), "The ARP table shows local IP-to-MAC mappings and tells you nothing beyond the LAN.", new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"), "arp -a" },
                    { new Guid("31c3bc11-b819-7b09-5b2e-a0566d7a5bcf"), "Signal strength has nothing to do with whether authentication succeeds.", new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"), "Confirm the Wi-Fi icon in the menu bar shows full signal" },
                    { new Guid("32b14c60-3d85-945e-678f-270489f30744"), "Without validation, Macs may send credentials to any rogue access point using that SSID.", new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"), "Turn off server certificate validation in the 802.1X profile so the Macs accept the renewed certificate" },
                    { new Guid("33af001a-5756-3db7-7097-d64d6aa8a6ca"), "Registration keeps signaling reachable. It doesn't change where the far end sends RTP.", new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"), "Shorten the SIP registration interval on all branch phones" },
                    { new Guid("33e8bdae-ff34-5d0e-3266-b6dd6dfcb2af"), "Name resolution isn't what's failing. Users connect, but the connection is unstable.", new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"), "Run nslookup for the cloud app's hostname from a client" },
                    { new Guid("37499301-a0fe-5c87-cd0d-32a8474207a2"), "With no evidence, the ISP is likely to start from the beginning or close the ticket.", new Guid("22105984-345f-f942-460c-ef2a7966ae39"), "Tell the ISP the internet is slow and request a callback" },
                    { new Guid("384479fb-34bd-3cb6-9343-26381a84d520"), "dig already got the new address, and Windows laptops are resolving it too.", new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"), "The authoritative DNS record hasn't been updated yet" },
                    { new Guid("40cce026-bde9-b83d-6a2d-88d6a040921e"), "The certificate is presented in the EAP exchange, not looked up through DNS.", new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"), "DNS caching delayed the new certificate from reaching the Macs" },
                    { new Guid("4437f3bb-a05e-aea9-44d9-0c1cf3436537"), "A ringing phone sends 180 Ringing, and that never appears in this ladder.", new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"), "The phone is ringing, but nobody is there to answer it" },
                    { new Guid("451ab605-0cf1-1f31-9833-c9d64fe27cf6"), "The loss begins well before the vendor's network, so the vendor can't fix it.", new Guid("22105984-345f-f942-460c-ef2a7966ae39"), "Open a ticket with the cloud app vendor about its server" },
                    { new Guid("464f613f-4523-c06f-b616-2390d3d25d16"), "This causes an outage, wipes evidence, and the local file server is already working fine.", new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"), "Restart the branch core switch to clear out its counters" },
                    { new Guid("46c923f5-2df5-14c0-1581-4de1a3560aa5"), "The INVITE is being dropped at the router, so waiting longer just makes the same failure slower.", new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"), "Raise Timer B on the PBX from 32 seconds to 120 seconds so the phone has more time to respond" },
                    { new Guid("4837be8e-c4f9-ce45-cec0-3d498221ef82"), "802.1X happens before an IP address is assigned, and these drops are authentication failures.", new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"), "Check the DHCP lease for each Mac on the staff VLAN" },
                    { new Guid("489c3817-f7c6-0a14-808b-e00816f438af"), "DNS is already answering correctly, so a TTL change can't affect this Mac's answer.", new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"), "Ask the DNS team to lower the record's TTL and wait for the old answer to age out of the cache" },
                    { new Guid("48c2a092-8f61-43e9-f208-6ea0c7054348"), "nslookup queries the DNS server directly, so it can't show you what the Mac's cache is holding.", new Guid("827bc225-d24a-2406-a175-96abdf975124"), "Run nslookup intranet.corp.example on the Mac, since it follows the same resolver path that apps use" },
                    { new Guid("4929832b-98fa-02ad-6bd1-cd36ea09c00a"), "The server is only where the loss ends up. It clearly starts several hops earlier.", new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"), "The destination server is overloaded and dropping requests" },
                    { new Guid("4c532a05-e017-9890-fd1e-c947466fea2f"), "Renewing asks for a lease again, and it fails the same way if the pool has none left.", new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"), "Run ipconfig /renew on each affected device in turn" },
                    { new Guid("52106692-9261-1ab6-2e57-5db1c31fe6d6"), "Traceroute sends only three probes per hop, which is too few to measure intermittent loss.", new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"), "traceroute -n -m 30 203.0.113.50" },
                    { new Guid("5d68785e-0606-6578-2934-a54b76563551"), "Windows laptops work on the same access points, so the hardware isn't the likely cause.", new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"), "Replace the access points that the affected Macs are using, starting with the busiest floor of the office" },
                    { new Guid("5f25aa21-9c88-c4ea-5feb-bbe2007062ed"), "A codec failure comes back as a response such as 488. Here there's no response of any kind.", new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"), "The two ends have no codec in common for this call" },
                    { new Guid("5f2f4070-04f6-deb2-80b5-f4c6633b7e51"), "ARP is separate from DHCP, and the scope statistics already explain the failure.", new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"), "The gateway's ARP cache is full and is blocking new devices" },
                    { new Guid("62960d03-1367-4418-316c-6ee37029a007"), "networksetup is a command-line tool, not a running cache daemon.", new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"), "sudo killall -9 networksetup" },
                    { new Guid("669daa4b-f0c1-3b69-da3c-9e45434d8e38"), "The first connection is useful, but it doesn't prove the 8-hour reauthentication passes.", new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"), "Check that a Mac connects again straight after the profile arrives, then close the ticket as resolved" },
                    { new Guid("68130180-089a-1428-e4af-9338a39010ac"), "The number isn't the problem. The new extension would hit the same expired mapping.", new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"), "Move the extension to a different number on the PBX" },
                    { new Guid("6a93a48c-94c8-110d-6d19-2dfc6ad946f0"), "A deliberate decline usually returns 603 Decline, or 486 Busy Here, not a timeout.", new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"), "The called user pressed decline on the handset" },
                    { new Guid("6ec397f0-5958-26ec-3d74-3673c533c591"), "No media is set up for a call that never connected, so there are no RTP statistics to read.", new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"), "Check the RTP packet statistics for the failed call on the PBX to see where audio stopped flowing" },
                    { new Guid("6f836da9-140a-e1ea-198b-a7648ec4175b"), "This phone already worked right after a reboot, so that test would pass even without a fix.", new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"), "Call the phone straight after you've rebooted it" },
                    { new Guid("7250e01a-749d-f73d-2146-d0bc98c9fae2"), "A small jitter buffer causes choppy audio from late packets. Here no return packets arrive at all.", new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"), "The jitter buffer on the phones is too small for this WAN link" },
                    { new Guid("73e2133a-a7ef-5c41-ba93-7b739cb964c2"), "This drops access to internal resources and leaves the failure in place.", new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"), "Move the MacBooks to the open guest SSID for the time being" },
                    { new Guid("78b1dff5-2554-98d4-ae1b-41e7dadf2a94"), "Expired passwords produce a credential rejection, and only after the TLS tunnel forms.", new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"), "The users' directory passwords expired overnight, so the RADIUS server is rejecting every login" },
                    { new Guid("81937004-5390-cb0b-f8f0-cecd0f34d58e"), "Each later hop just inherits loss from upstream, so read from where it starts.", new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"), "The final router before the destination is at fault because it's the last hop to show any loss" },
                    { new Guid("81d30037-002b-6cf2-806f-a882ffb8fa80"), "DNS resolves names and has no part in assigning a client's IP address.", new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"), "A DNS server has handed out an address from the wrong zone" },
                    { new Guid("85469eeb-f024-53a6-6081-f43cf1953fab"), "Finder isn't involved in DNS, and disabling system services can cause other problems.", new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"), "sudo launchctl disable system/com.apple.Finder; sudo killall Finder" },
                    { new Guid("8efd53bc-b6e1-67a8-001d-2e9d1a9c2043"), "A host blocking ICMP would show 100% loss. It wouldn't show a clean 0%.", new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"), "The destination is hiding its true loss by blocking ICMP" },
                    { new Guid("900ed6e7-4725-34d8-8c97-090d24bb1b5d"), "This discards the lease records and risks duplicate addresses with clients still using them.", new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"), "Delete the scope and recreate it from scratch during the day so that every lease is released at once" },
                    { new Guid("925deedf-6d8f-5bea-39f0-dda6b2cea49e"), "That's a drastic step for a problem that one text file or a resolver entry can explain.", new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"), "Reinstall macOS to restore the default network configuration" },
                    { new Guid("926a8855-0980-6959-4889-e26ecd6f0711"), "This shows the interface's addressing. It tells you nothing about how a name resolves.", new Guid("827bc225-d24a-2406-a175-96abdf975124"), "Run ifconfig en0 to look at the Mac's interface details" },
                    { new Guid("926b41ce-23b9-27a4-651d-3b1db6dcab8f"), "An overloaded server normally answers 503 Service Unavailable, and other extensions work fine.", new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"), "The PBX is overloaded and is shedding incoming calls, so it's resending each INVITE until one gets through" },
                    { new Guid("92a56360-1409-ac16-ba68-ce9a51828199"), "With no common codec the call normally fails during setup, often with 488 Not Acceptable Here. It wouldn't connect and carry one good audio direction.", new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"), "The phones and the hosted PBX have a codec mismatch, so audio can't be decoded on one side" },
                    { new Guid("96b1163e-d19b-f860-bd21-54a6e695a26e"), "WPA2-Enterprise uses 802.1X per user and has no pre-shared key.", new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"), "The Wi-Fi pre-shared key was rotated on the access points" },
                    { new Guid("99e42082-f570-da27-e005-d0f238a95d15"), "Port 5060 carries SIP signaling, which already works. RTP uses a range of UDP ports.", new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"), "Open TCP port 5060 inbound from the hosted PBX to the phones so their audio can reach the branch" },
                    { new Guid("9ad82821-024e-7266-7fe7-2aadbdc85b00"), "Its interface is clean, so this is expensive and doesn't address the evidence.", new Guid("22105984-345f-f942-460c-ef2a7966ae39"), "Replace the firewall, since it's the last device you control before traffic reaches the ISP" },
                    { new Guid("9b4cf64a-f34f-07a6-3626-5ba126065c9e"), "Outbound calls always worked. The problem is only with inbound calls.", new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"), "Have the remote worker place an outbound test call" },
                    { new Guid("9d998a4f-3723-a756-964d-ba6fb14e3098"), "Wired and wireless clients on the VLAN share the same symptom, and existing clients work.", new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"), "Replace the wireless access points serving the floor" },
                    { new Guid("9ed84fd8-0d1c-cc85-945c-daeb56bd6573"), "You don't control visitors' devices, and the pool would still be too small.", new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"), "Turn off the private MAC address feature on visitors' phones" },
                    { new Guid("a286d2a4-79c8-e470-caed-6afecda31177"), "Every phone at the site failing at once, right after a firewall change, points to the network and not to hardware.", new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"), "The handsets' earpieces need to be replaced at the branch" },
                    { new Guid("a46b90b1-cb89-d9e0-c063-acd68f8f905b"), "Static addresses don't scale for visitors, and they would collide with addresses the scope hands out.", new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"), "Assign static IP addresses to each device that can't connect" },
                    { new Guid("a8894e10-df47-cc86-3e3e-b4d558dfae3a"), "Teaching users to accept certificate prompts exposes them to fake access points.", new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"), "Tell users to click Trust when the certificate prompt appears" },
                    { new Guid("b06b7742-39a4-a7c1-0bb4-b2d0a9aecaf6"), "A rewriting router would affect dig as well, and Windows laptops on it work.", new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"), "The office router is intercepting DNS and rewriting responses for this one host on the Mac's subnet" },
                    { new Guid("b1e45d07-979b-3902-e503-2741ba309f34"), "This lists local sockets and their state, not the loss along a network path.", new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"), "netstat -an" },
                    { new Guid("b5698b19-2b04-dc0e-52eb-84d4371a1aef"), "It showed as registered during the failures too, so the icon proves nothing.", new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"), "Confirm that the phone is displaying a green registered icon" },
                    { new Guid("b6326f9b-e01d-bd56-b5fd-6ef99ede8435"), "An unknown destination returns 404 Not Found, usually straight away rather than after 32 seconds.", new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"), "The PBX searched its dial plan, found no such extension, and rejected the call with a timeout" },
                    { new Guid("b6f40cb7-251b-602a-aec4-1885e234d01e"), "Internal calls may stay on the local network and never cross the NAT path that was failing.", new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"), "Place a single internal extension-to-extension test call" },
                    { new Guid("ba01ef5d-d0f7-7d8f-b1e8-d4a4f871d2d4"), "Outbound calls work, so the phone isn't obviously broken, and swapping it skips the diagnosis.", new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"), "Replace the remote worker's handset with a spare phone" },
                    { new Guid("c491f14c-8826-4811-4196-b996ec9498bd"), "The phones show as registered and can make and receive calls, so registration isn't the problem.", new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"), "SIP registration is failing intermittently at the branch" },
                    { new Guid("c813ea71-7d70-9625-ab7b-0349cf2af743"), "That's a Windows command. On macOS, ipconfig is a different tool and has no such option.", new Guid("827bc225-d24a-2406-a175-96abdf975124"), "Run ipconfig /flushdns in Terminal and check the site again" },
                    { new Guid("c858e2cb-1469-9c66-afb5-356c113f6c6c"), "Missing QoS markings lower priority under congestion. They don't remove one direction of audio completely.", new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"), "DSCP markings are missing from the voice traffic, so the ISP is dropping the return RTP packets" },
                    { new Guid("c983fd3b-418a-e349-7b51-f830a9a4ac4d"), "Hops 1 and 2 show 0%, so traffic is getting through the LAN and the firewall cleanly.", new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"), "The LAN switch is dropping frames between clients and the firewall" },
                    { new Guid("c9d00001-424d-2969-02c8-7edb0c002651"), "A rising counter shows traffic matched the rule, not that a person can hear audio in each direction.", new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"), "Check that the firewall rule's hit counter is going up on the new RTP rule during several live calls" },
                    { new Guid("cdf11c38-ab40-41a4-15ed-a89485863cc3"), "Revocation isn't what's failing, since the client reports an unknown CA.", new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"), "macOS checks certificates for revocation on a fixed daily schedule" },
                    { new Guid("d21ace62-4b81-8481-8528-345ec14ebcae"), "Alerts are worth adding, but they only warn you. They don't add a single address.", new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"), "Set a scope threshold alert and make no other changes" },
                    { new Guid("d25780cf-6fae-db32-e489-dcc369b881c8"), "Jumbo frames generally don't work across an ISP link, and this could add new faults.", new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"), "Raise the WAN interface MTU to 9000 to improve throughput" },
                    { new Guid("ddb0ccd8-87ee-f97b-f1e9-aa6b47f69582"), "Loss isn't additive across hops, and the destination itself shows 0%.", new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"), "Add every hop's figure together to get a 40% end-to-end loss" },
                    { new Guid("df68bc1e-9c7b-fbe7-09df-ac9a146ef0cb"), "Later hops prove traffic is passing through hop 5 cleanly, so that ticket would be rejected.", new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"), "Hop 5 is dropping real traffic, so open a priority ticket with the network that owns that router" },
                    { new Guid("e09b14cf-c1ae-8474-02f0-f39c3c0b090b"), "This destroys the evidence, affects every user, and doesn't explain a single phone's failure.", new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"), "Reboot the PBX during a quiet period and retest the call" },
                    { new Guid("e3cdfc1b-fcfc-413e-38d5-35e46137a36e"), "NAT doesn't assign addresses to clients, and 169.254.x.x isn't a NAT range.", new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"), "The default gateway is performing NAT for the guest VLAN" },
                    { new Guid("e40a12c1-8d60-2324-7be9-d0af906cb580"), "Ping proves basic reachability with ICMP. It says nothing about which addresses and ports RTP is using.", new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"), "Run a sustained ping from a phone's VLAN to the hosted PBX address" },
                    { new Guid("e4a3db7a-843d-0dfa-6338-e78bc751262a"), "A workaround may help a single user, but it doesn't diagnose anything or restore the site.", new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"), "Have users switch to a mobile hotspot until the loss clears up" },
                    { new Guid("e8b12b8b-66a0-e481-e95f-ef5f4f7a9b62"), "Leases persist across a restart, so this interrupts service and frees nothing.", new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"), "Restart the DHCP Server service with Restart-Service DHCPServer to clear out the address pool" },
                    { new Guid("ebea3b74-0c06-f68b-f8f2-e769ed88a873"), "A DHCP reservation would give an address from the scope, not a 169.254.x.x address.", new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"), "The client was given a reserved address by the DHCP server because its MAC is in the exclusion list" },
                    { new Guid("eda27977-a3ce-a622-4683-d0b6c34ead2a"), "Authentication challenges return 401 or 407, and they arrive immediately.", new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"), "The caller failed digest authentication on the PBX" },
                    { new Guid("f0863aa5-66cc-76be-49f6-e084db312479"), "Four packets can't measure intermittent loss, and one target doesn't tell you where the loss is.", new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"), "Ping 203.0.113.50 with the default four packets, since that's the address users are complaining about" },
                    { new Guid("f2bd19ba-2006-031b-52a6-b115d4ac715c"), "This exposes the phone to the internet and is hard to maintain, when a keep-alive solves the actual cause.", new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"), "Port-forward all UDP ports on the home router to the phone" },
                    { new Guid("f3a90ba2-3f7a-e093-c878-65f84b1c130b"), "The lease list is readable and accurate, and it shows real demand.", new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"), "The DHCP server's database has become corrupted" },
                    { new Guid("fb413b5a-56a3-3d6a-afbd-3cfd8e097380"), "With no router interface in that subnet, those clients couldn't reach anything.", new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"), "Add a second scope in another subnet with no gateway address for it" },
                    { new Guid("fbad0072-7ed7-c0b0-b4b4-3f0a2033a9be"), "A key mismatch would show RTP packets arriving but undecodable. This capture shows no return stream at all.", new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"), "The SRTP keys don't match, so the phone is discarding the audio" },
                    { new Guid("ff9e9e1d-8323-1da5-cd51-77baef6e4eeb"), "Changing the codec changes how audio is encoded, not the unreachable address in the SDP.", new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"), "Force every phone to use the G.711 codec instead of G.729" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0cc138cb-e592-59b2-9214-cbfddff702ed"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0d366d09-90de-d046-2dbc-7322c6ff8042"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("11543112-48f0-da51-1ede-9bba39fce770"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("11d8b8bb-c299-c006-4cf6-39862c4b0711"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1632807d-feb0-35b5-94aa-15964031bec8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("199754d9-8bee-6eea-2d2c-534802a69c38"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1b2782a6-3845-b728-6e7f-ad6bc2ead369"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1c186583-f952-1da2-382c-ac7770a62b7f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1d7f1cc4-08cd-e5b0-536e-b118927e576b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1ef30f6d-e145-2a62-9f7f-663f8beff3dd"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("1f1c546e-edfa-02f9-a196-7079d261c208"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2320f06f-c909-4044-bd50-b060889a0c58"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("23aac207-a651-d480-15c4-15a0e7b2e0af"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2d5856ff-6304-b4a3-617f-e89b79261b65"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("31c3bc11-b819-7b09-5b2e-a0566d7a5bcf"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("32b14c60-3d85-945e-678f-270489f30744"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("33af001a-5756-3db7-7097-d64d6aa8a6ca"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("33e8bdae-ff34-5d0e-3266-b6dd6dfcb2af"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("37499301-a0fe-5c87-cd0d-32a8474207a2"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("384479fb-34bd-3cb6-9343-26381a84d520"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("40cce026-bde9-b83d-6a2d-88d6a040921e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4437f3bb-a05e-aea9-44d9-0c1cf3436537"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("451ab605-0cf1-1f31-9833-c9d64fe27cf6"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("464f613f-4523-c06f-b616-2390d3d25d16"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("46c923f5-2df5-14c0-1581-4de1a3560aa5"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4837be8e-c4f9-ce45-cec0-3d498221ef82"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("489c3817-f7c6-0a14-808b-e00816f438af"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("48c2a092-8f61-43e9-f208-6ea0c7054348"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4929832b-98fa-02ad-6bd1-cd36ea09c00a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("4c532a05-e017-9890-fd1e-c947466fea2f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("52106692-9261-1ab6-2e57-5db1c31fe6d6"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5d68785e-0606-6578-2934-a54b76563551"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5f25aa21-9c88-c4ea-5feb-bbe2007062ed"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5f2f4070-04f6-deb2-80b5-f4c6633b7e51"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("62960d03-1367-4418-316c-6ee37029a007"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("669daa4b-f0c1-3b69-da3c-9e45434d8e38"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("68130180-089a-1428-e4af-9338a39010ac"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6a93a48c-94c8-110d-6d19-2dfc6ad946f0"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6ec397f0-5958-26ec-3d74-3673c533c591"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("6f836da9-140a-e1ea-198b-a7648ec4175b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("7250e01a-749d-f73d-2146-d0bc98c9fae2"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("73e2133a-a7ef-5c41-ba93-7b739cb964c2"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("78b1dff5-2554-98d4-ae1b-41e7dadf2a94"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("81937004-5390-cb0b-f8f0-cecd0f34d58e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("81d30037-002b-6cf2-806f-a882ffb8fa80"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("85469eeb-f024-53a6-6081-f43cf1953fab"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8efd53bc-b6e1-67a8-001d-2e9d1a9c2043"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("900ed6e7-4725-34d8-8c97-090d24bb1b5d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("925deedf-6d8f-5bea-39f0-dda6b2cea49e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("926a8855-0980-6959-4889-e26ecd6f0711"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("926b41ce-23b9-27a4-651d-3b1db6dcab8f"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("92a56360-1409-ac16-ba68-ce9a51828199"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("96b1163e-d19b-f860-bd21-54a6e695a26e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("99e42082-f570-da27-e005-d0f238a95d15"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9ad82821-024e-7266-7fe7-2aadbdc85b00"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9b4cf64a-f34f-07a6-3626-5ba126065c9e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9d998a4f-3723-a756-964d-ba6fb14e3098"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("9ed84fd8-0d1c-cc85-945c-daeb56bd6573"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a286d2a4-79c8-e470-caed-6afecda31177"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a46b90b1-cb89-d9e0-c063-acd68f8f905b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a8894e10-df47-cc86-3e3e-b4d558dfae3a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b06b7742-39a4-a7c1-0bb4-b2d0a9aecaf6"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b1e45d07-979b-3902-e503-2741ba309f34"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b5698b19-2b04-dc0e-52eb-84d4371a1aef"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b6326f9b-e01d-bd56-b5fd-6ef99ede8435"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("b6f40cb7-251b-602a-aec4-1885e234d01e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ba01ef5d-d0f7-7d8f-b1e8-d4a4f871d2d4"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c491f14c-8826-4811-4196-b996ec9498bd"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c813ea71-7d70-9625-ab7b-0349cf2af743"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c858e2cb-1469-9c66-afb5-356c113f6c6c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c983fd3b-418a-e349-7b51-f830a9a4ac4d"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("c9d00001-424d-2969-02c8-7edb0c002651"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("cdf11c38-ab40-41a4-15ed-a89485863cc3"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d21ace62-4b81-8481-8528-345ec14ebcae"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("d25780cf-6fae-db32-e489-dcc369b881c8"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ddb0ccd8-87ee-f97b-f1e9-aa6b47f69582"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("df68bc1e-9c7b-fbe7-09df-ac9a146ef0cb"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e09b14cf-c1ae-8474-02f0-f39c3c0b090b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e3cdfc1b-fcfc-413e-38d5-35e46137a36e"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e40a12c1-8d60-2324-7be9-d0af906cb580"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e4a3db7a-843d-0dfa-6338-e78bc751262a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e8b12b8b-66a0-e481-e95f-ef5f4f7a9b62"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ebea3b74-0c06-f68b-f8f2-e769ed88a873"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("eda27977-a3ce-a622-4683-d0b6c34ead2a"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f0863aa5-66cc-76be-49f6-e084db312479"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f2bd19ba-2006-031b-52a6-b115d4ac715c"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("f3a90ba2-3f7a-e093-c878-65f84b1c130b"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fb413b5a-56a3-3d6a-afbd-3cfd8e097380"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fbad0072-7ed7-c0b0-b4b4-3f0a2033a9be"));

            migrationBuilder.DeleteData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ff9e9e1d-8323-1da5-cd51-77baef6e4eeb"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("05b59863-e25d-c8b4-6d55-b1a8d1aa1d8b"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("168538da-791e-8797-6e3b-6ef8ef1967ae"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("1b4e9af1-6bd6-e9be-bb73-7783c80fd720"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("22105984-345f-f942-460c-ef2a7966ae39"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("289ddf82-0d83-5ef9-be65-7555c81292a5"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("44d94fa5-e61d-069d-cfb9-924d08432914"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("47871ad9-ca0a-3561-703e-4013a846f0ff"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4bdf0807-333d-6f83-56ef-401d5e3c7539"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("4fb282d0-d4f4-ed6d-1709-4aac04ae40f6"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("52ce1b34-42bb-920e-f7df-5b25a7b5f887"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6d7f1865-4c10-3114-9e5a-22d813c5a1a8"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6df6fd12-6ad1-216d-b137-9d14be120a0c"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("6ed8f58a-a744-11bb-2126-12be215bda38"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("727d0862-2c61-a12c-dcce-0eda318276cd"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("827bc225-d24a-2406-a175-96abdf975124"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("8a7e8678-b58e-7a7e-5a25-03b9887dde9b"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("9d2372ef-5c7c-50c9-2269-c4dbad4d333a"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a10c9f50-2ca5-f40b-8205-c945ecfdf9b9"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a1516d18-5455-b304-22e4-45110fd25ad4"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("a27dfa65-41e1-a147-3929-a1645ab27517"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ad2cf786-3bfd-48f9-51d0-aa203e8dae0c"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("aeb378c3-7bc3-be9c-01c0-28bca1357a1d"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("b8a8f230-7cd2-3774-3659-42c5c02bdd07"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c727d9c4-6194-7a90-8830-55d7a0befbe9"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("c7faa051-2a06-39ce-1657-2d77bd0ce29d"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("ddfc9042-5cc7-104f-6975-f66f1185f5ff"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("de253677-1db2-549e-2dc2-35fc21a057ca"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("e527425e-6370-df76-1dc4-db9ec3a8d762"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("eafc22a2-6edc-9744-e2b3-64a8d6a4f190"));

            migrationBuilder.DeleteData(
                table: "SequentialSteps",
                keyColumn: "Id",
                keyValue: new Guid("f4184cb1-5cc2-308c-718a-94b03094ae58"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("1b904159-4dbb-c44f-8839-9e0df34b7b47"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("2a935101-a258-07c0-d4cf-7d4141dd7a76"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("6f34a4ec-fd2b-e8f6-1e43-9a284dd34538"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("95e42524-b675-8aab-3bae-f938012f09ed"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("cd16467d-d0a1-c6dd-0137-f60b5f5aeb29"));

            migrationBuilder.DeleteData(
                table: "ScenarioCards",
                keyColumn: "Id",
                keyValue: new Guid("e5b5a9b9-099b-c951-5fe7-002659158d1c"));
        }
    }
}
