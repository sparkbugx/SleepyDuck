using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleepyDuck.Api.Migrations
{
    /// <inheritdoc />
    public partial class RebalanceM365Options : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0603fb52-4fd8-be34-9b97-169203ff4b2d"),
                column: "Text",
                value: "Add northwind-supply.example to the allowed domains in the anti-spam policy that covers Priya, so future mail from the supplier isn't filtered as spam again");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("095f4ce6-690b-0727-bddf-250c4e7485a3"),
                column: "Text",
                value: "Review the Exchange admin center for the mailbox, then confirm that its settings match what HR asked for");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("169da1fd-62ba-c18a-4479-dc6384fd1838"),
                column: "Text",
                value: "Run a content search for 'invoice' across Priya's mailbox in Microsoft Purview, using the supplier's domain and a date range covering the missing invoices");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2e625d6b-46d1-2e6c-205b-9c350fcd4f48"),
                column: "Text",
                value: "Create a new inbound connector for the partners, so that the group accepts their domains");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2e894765-4a93-5534-d0dd-0114d14519e2"),
                column: "Text",
                value: "In Microsoft Purview Audit, by searching for failed logons from those three users since the assignment ran");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3a3d9577-8936-8ded-7251-0257cd724c08"),
                column: "Text",
                value: "Whether the disclaimer text is over a length limit, or contains HTML that the rule can't insert into the vendor's messages");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("432b768a-c896-bc58-0743-bdea76bf0f1f"),
                column: "Text",
                value: "Check the affected users' Outlook view settings, in case the disclaimer is collapsed or hidden in the reading pane");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("480e3db2-90e4-0621-c76d-f5414350be80"),
                column: "Text",
                value: "Exchange admin center > Recipients > Contacts > Add a mail contact");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5abd7981-6646-8145-b3a3-cc769e3b12e0"),
                column: "Text",
                value: "Ask the supplier to resend the invoices, then wait for the new copies to clear filtering and confirm that Priya has them");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5f205bc1-bed0-db3f-5c2d-03a8c2a13b90"),
                column: "Text",
                value: "Add a second group with Viva Engage turned off, and put the same Marketing users in both groups");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("61bda9c7-34ac-d550-fdd3-a7319bf05092"),
                column: "Text",
                value: "Enforce the rule immediately with it enabled, then watch user reports through the first full day");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("67888402-2051-bd66-00f9-0079f63b97ce"),
                column: "Text",
                value: "Check Marco's Sent Items for the forwarded copy after the next matching message triggers the rule");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("727e05c8-8193-bad1-0619-40f9842c2ea9"),
                column: "Text",
                value: "Block Dana from SharePoint and Viva Engage with a Conditional Access policy, and leave the full E3 license in place");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("824d44bb-63f6-a319-802f-5088d59af059"),
                column: "Text",
                value: "A Global Administrator as the only member, so that membership changes stay with IT");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8c9bc4b7-03a3-6c2c-75e8-e176e87ec6b7"),
                column: "Text",
                value: "Add-MailboxPermission -Identity support@contoso.example -User aisha@contoso.example -AccessRights FullAccess -InheritanceType All");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a64b880d-2f0a-88ce-f1cb-7981d14a22fd"),
                column: "Text",
                value: "Assign the license to a distribution list that HR keeps up to date for the Marketing department");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ba821093-8f7d-670a-1b4b-0ee79d40a8c4"),
                column: "Text",
                value: "Set automatic forwarding to On in the Default outbound policy, since that's the policy the mailbox is using today");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("bb26846f-02ba-26f2-0575-173704c0687c"),
                column: "Text",
                value: "An Outlook inbox rule on each mailbox, deployed with a script");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("dd32efab-f827-efed-fe0a-dd5185c4bf7f"),
                column: "Text",
                value: "SharePoint admin center > Term store, then add a term set for the project team and assign its members to it");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e1cc9405-3972-a565-51a0-4edb6be76966"),
                column: "Text",
                value: "The partner's mail server rejected Marco's domain, which is why the bounce came back with a 5.7 code");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e27f46d5-3270-cc9e-dedf-5c80c3a73f19"),
                column: "Text",
                value: "Ask the supplier to resend the invoice from a personal mailbox, so that Priya at least gets it today");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fadf85f5-6e57-973c-868c-a91315e9eac5"),
                column: "Text",
                value: "Microsoft 365 admin center > Roles > Role assignments, then assign Aisha the Exchange Recipient Administrator role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("0603fb52-4fd8-be34-9b97-169203ff4b2d"),
                column: "Text",
                value: "Add northwind-supply.example to the allowed domains in the anti-spam policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("095f4ce6-690b-0727-bddf-250c4e7485a3"),
                column: "Text",
                value: "Review the Exchange admin center for the mailbox");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("169da1fd-62ba-c18a-4479-dc6384fd1838"),
                column: "Text",
                value: "Run a content search for 'invoice' in Microsoft Purview");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2e625d6b-46d1-2e6c-205b-9c350fcd4f48"),
                column: "Text",
                value: "Create a new inbound connector for the partners");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("2e894765-4a93-5534-d0dd-0114d14519e2"),
                column: "Text",
                value: "In Microsoft Purview Audit, by searching for failed logons");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("3a3d9577-8936-8ded-7251-0257cd724c08"),
                column: "Text",
                value: "Whether the disclaimer text is over a length limit");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("432b768a-c896-bc58-0743-bdea76bf0f1f"),
                column: "Text",
                value: "Check the affected users' Outlook view settings");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("480e3db2-90e4-0621-c76d-f5414350be80"),
                column: "Text",
                value: "Exchange admin center > Recipients > Contacts");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5abd7981-6646-8145-b3a3-cc769e3b12e0"),
                column: "Text",
                value: "Ask the supplier to resend and wait for it to clear");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("5f205bc1-bed0-db3f-5c2d-03a8c2a13b90"),
                column: "Text",
                value: "Add a second group that removes the plan");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("61bda9c7-34ac-d550-fdd3-a7319bf05092"),
                column: "Text",
                value: "Enforce immediately and wait for user complaints");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("67888402-2051-bd66-00f9-0079f63b97ce"),
                column: "Text",
                value: "Check Marco's Sent Items for the forwarded copy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("727e05c8-8193-bad1-0619-40f9842c2ea9"),
                column: "Text",
                value: "Block Dana with a Conditional Access policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("824d44bb-63f6-a319-802f-5088d59af059"),
                column: "Text",
                value: "A Global Administrator as the only member");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("8c9bc4b7-03a3-6c2c-75e8-e176e87ec6b7"),
                column: "Text",
                value: "Add-MailboxPermission -Identity support@contoso.example -User aisha@contoso.example -AccessRights FullAccess");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("a64b880d-2f0a-88ce-f1cb-7981d14a22fd"),
                column: "Text",
                value: "Assign the license to a distribution list");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("ba821093-8f7d-670a-1b4b-0ee79d40a8c4"),
                column: "Text",
                value: "Set automatic forwarding to On in the Default outbound policy");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("bb26846f-02ba-26f2-0575-173704c0687c"),
                column: "Text",
                value: "An Outlook inbox rule on each mailbox");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("dd32efab-f827-efed-fe0a-dd5185c4bf7f"),
                column: "Text",
                value: "SharePoint admin center > Term store");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e1cc9405-3972-a565-51a0-4edb6be76966"),
                column: "Text",
                value: "The partner's mail server rejected Marco's domain");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("e27f46d5-3270-cc9e-dedf-5c80c3a73f19"),
                column: "Text",
                value: "Ask the supplier to resend the invoice from a personal mailbox");

            migrationBuilder.UpdateData(
                table: "Distractors",
                keyColumn: "Id",
                keyValue: new Guid("fadf85f5-6e57-973c-868c-a91315e9eac5"),
                column: "Text",
                value: "Microsoft 365 admin center > Roles > Role assignments");
        }
    }
}
