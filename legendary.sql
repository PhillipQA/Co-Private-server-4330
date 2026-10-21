-- Legendary gear: new items and the Legendsmith NPC.
-- Run once on an existing database (fresh installs get this from seed.sql):
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -d Conquer -i legendary.sql
-- Also run `dotnet ef database update --project Conquer.Game` to add the Items.Legendary column.

SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;

BEGIN TRANSACTION;

SET IDENTITY_INSERT [ItemTypes] ON;
INSERT INTO [ItemTypes] ([Id], [Name], [RequiredProfession], [RequiredWeaponSkill], [RequiredLevel], [RequiredSex], [RequiredStrength], [RequiredAgility], [RequiredVitality], [RequiredSpirit], [Restrictions], [Weight], [Price], [Task], [AttackMax], [AttackMin], [Defense], [Agility], [Dodge], [Health], [Mana], [Durability], [MaxDurability], [Status], [Gem1], [Gem2], [Magic1], [Magic2], [Plus], [MagicAttack], [MagicDefense], [AttackRange], [AttackSpeed])
SELECT * FROM (VALUES
(1088100, N'DragonShard', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(1088101, N'SuperDragonBall', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(1088102, N'LegendOrb', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 300000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
) AS new_items ([Id], [Name], [RequiredProfession], [RequiredWeaponSkill], [RequiredLevel], [RequiredSex], [RequiredStrength], [RequiredAgility], [RequiredVitality], [RequiredSpirit], [Restrictions], [Weight], [Price], [Task], [AttackMax], [AttackMin], [Defense], [Agility], [Dodge], [Health], [Mana], [Durability], [MaxDurability], [Status], [Gem1], [Gem2], [Magic1], [Magic2], [Plus], [MagicAttack], [MagicDefense], [AttackRange], [AttackSpeed])
WHERE NOT EXISTS (SELECT 1 FROM [ItemTypes] WHERE [ItemTypes].[Id] = new_items.[Id]);
SET IDENTITY_INSERT [ItemTypes] OFF;

-- The Legendsmith stands in Twin City near the Blacksmith. Change Look/X/Y to taste.
SET IDENTITY_INSERT [Npcs] ON;
INSERT INTO [Npcs] ([Id], [Name], [Type], [Look], [Base], [Sort], [MapId], [X], [Y])
SELECT 10777, N'Legendsmith', 2, 7360, 0, 0, 1002, 447, 331
WHERE NOT EXISTS (SELECT 1 FROM [Npcs] WHERE [Id] = 10777);
SET IDENTITY_INSERT [Npcs] OFF;

COMMIT;
