using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conquer.Game.Migrations;

/// <inheritdoc />
public partial class _20260927191356_Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Bounties",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                Target = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                Payer = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                Reward = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Bounties", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DynaNpcs",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                OwnerId = table.Column<long>(type: "bigint", nullable: false),
                OwnerType = table.Column<byte>(type: "tinyint", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Type = table.Column<byte>(type: "tinyint", nullable: false),
                Look = table.Column<long>(type: "bigint", nullable: false),
                MaxHealth = table.Column<long>(type: "bigint", nullable: false),
                Base = table.Column<byte>(type: "tinyint", nullable: false),
                Sort = table.Column<byte>(type: "tinyint", nullable: false),
                Defense = table.Column<int>(type: "int", nullable: false),
                MagicDefense = table.Column<byte>(type: "tinyint", nullable: false),
                LinkId = table.Column<long>(type: "bigint", nullable: false),
                Task0 = table.Column<long>(type: "bigint", nullable: false),
                Data0 = table.Column<int>(type: "int", nullable: false),
                Data1 = table.Column<int>(type: "int", nullable: false),
                Data2 = table.Column<int>(type: "int", nullable: false),
                Data3 = table.Column<int>(type: "int", nullable: false),
                Statue_Look = table.Column<long>(type: "bigint", nullable: true),
                Statue_Hair = table.Column<int>(type: "int", nullable: true),
                Statue_Head = table.Column<long>(type: "bigint", nullable: true),
                Statue_Armor = table.Column<long>(type: "bigint", nullable: true),
                Statue_Right = table.Column<long>(type: "bigint", nullable: true),
                Statue_Left = table.Column<long>(type: "bigint", nullable: true),
                Statue_Frame = table.Column<int>(type: "int", nullable: true),
                Statue_Pose = table.Column<byte>(type: "tinyint", nullable: true),
                Statue_Rank = table.Column<byte>(type: "tinyint", nullable: true),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<int>(type: "int", nullable: false),
                Y = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DynaNpcs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Generators",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<int>(type: "int", nullable: false),
                Y = table.Column<int>(type: "int", nullable: false),
                Width = table.Column<int>(type: "int", nullable: false),
                Height = table.Column<int>(type: "int", nullable: false),
                MaxMonsters = table.Column<int>(type: "int", nullable: false),
                RespawnSeconds = table.Column<int>(type: "int", nullable: false),
                MaxPerWave = table.Column<int>(type: "int", nullable: false),
                MonsterTypeId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Generators", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Guilds",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                Fund = table.Column<long>(type: "bigint", nullable: false),
                ParentGuildId = table.Column<long>(type: "bigint", nullable: true),
                Announcement = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Guilds", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ItemPluses",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ItemTypeId = table.Column<long>(type: "bigint", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Health = table.Column<short>(type: "smallint", nullable: false),
                AttackMax = table.Column<short>(type: "smallint", nullable: false),
                AttackMin = table.Column<short>(type: "smallint", nullable: false),
                Defense = table.Column<short>(type: "smallint", nullable: false),
                MagicAttack = table.Column<short>(type: "smallint", nullable: false),
                MagicDefense = table.Column<short>(type: "smallint", nullable: false),
                Agility = table.Column<short>(type: "smallint", nullable: false),
                Dodge = table.Column<short>(type: "smallint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ItemPluses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ItemTypes",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RequiredProfession = table.Column<byte>(type: "tinyint", nullable: false),
                RequiredWeaponSkill = table.Column<byte>(type: "tinyint", nullable: false),
                RequiredLevel = table.Column<byte>(type: "tinyint", nullable: false),
                RequiredSex = table.Column<byte>(type: "tinyint", nullable: false),
                RequiredStrength = table.Column<int>(type: "int", nullable: false),
                RequiredAgility = table.Column<int>(type: "int", nullable: false),
                RequiredVitality = table.Column<int>(type: "int", nullable: false),
                RequiredSpirit = table.Column<int>(type: "int", nullable: false),
                Restrictions = table.Column<byte>(type: "tinyint", nullable: false),
                Weight = table.Column<int>(type: "int", nullable: false),
                Price = table.Column<long>(type: "bigint", nullable: false),
                Task = table.Column<int>(type: "int", nullable: false),
                AttackMax = table.Column<int>(type: "int", nullable: false),
                AttackMin = table.Column<int>(type: "int", nullable: false),
                Defense = table.Column<short>(type: "smallint", nullable: false),
                Agility = table.Column<short>(type: "smallint", nullable: false),
                Dodge = table.Column<short>(type: "smallint", nullable: false),
                Health = table.Column<short>(type: "smallint", nullable: false),
                Mana = table.Column<short>(type: "smallint", nullable: false),
                Durability = table.Column<int>(type: "int", nullable: false),
                MaxDurability = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<byte>(type: "tinyint", nullable: false),
                Gem1 = table.Column<byte>(type: "tinyint", nullable: false),
                Gem2 = table.Column<byte>(type: "tinyint", nullable: false),
                Magic1 = table.Column<byte>(type: "tinyint", nullable: false),
                Magic2 = table.Column<byte>(type: "tinyint", nullable: false),
                Plus = table.Column<byte>(type: "tinyint", nullable: false),
                MagicAttack = table.Column<int>(type: "int", nullable: false),
                MagicDefense = table.Column<int>(type: "int", nullable: false),
                AttackRange = table.Column<int>(type: "int", nullable: false),
                AttackSpeed = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ItemTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "LevelExps",
            columns: table => new
            {
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Experience = table.Column<long>(type: "bigint", nullable: false),
                LevelUpTime = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LevelExps", x => x.Level);
            });

        migrationBuilder.CreateTable(
            name: "MagicTypes",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Type = table.Column<int>(type: "int", nullable: false),
                Sort = table.Column<byte>(type: "tinyint", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Offensive = table.Column<byte>(type: "tinyint", nullable: false),
                GroundTargeted = table.Column<byte>(type: "tinyint", nullable: false),
                MultiTarget = table.Column<byte>(type: "tinyint", nullable: false),
                Target = table.Column<long>(type: "bigint", nullable: false),
                Level = table.Column<int>(type: "int", nullable: false),
                ManaCost = table.Column<int>(type: "int", nullable: false),
                Power = table.Column<int>(type: "int", nullable: false),
                CastTime = table.Column<int>(type: "int", nullable: false),
                SuccessChance = table.Column<byte>(type: "tinyint", nullable: false),
                DurationSeconds = table.Column<long>(type: "bigint", nullable: false),
                Range = table.Column<byte>(type: "tinyint", nullable: false),
                CastRange = table.Column<byte>(type: "tinyint", nullable: false),
                Status = table.Column<long>(type: "bigint", nullable: false),
                RequiredProfession = table.Column<int>(type: "int", nullable: false),
                RequiredExp = table.Column<long>(type: "bigint", nullable: false),
                RequiredLevel = table.Column<byte>(type: "tinyint", nullable: false),
                XpSkill = table.Column<byte>(type: "tinyint", nullable: false),
                WeaponSubtype = table.Column<int>(type: "int", nullable: false),
                ActiveTimes = table.Column<long>(type: "bigint", nullable: false),
                AutoActive = table.Column<byte>(type: "tinyint", nullable: false),
                FloorAttr = table.Column<long>(type: "bigint", nullable: false),
                AutoLearn = table.Column<byte>(type: "tinyint", nullable: false),
                LearnLevel = table.Column<int>(type: "int", nullable: false),
                DropWeapon = table.Column<byte>(type: "tinyint", nullable: false),
                StaminaCost = table.Column<byte>(type: "tinyint", nullable: false),
                WeaponHit = table.Column<byte>(type: "tinyint", nullable: false),
                UseItem = table.Column<long>(type: "bigint", nullable: false),
                FollowUp = table.Column<int>(type: "int", nullable: false),
                FollowUpDelay = table.Column<int>(type: "int", nullable: false),
                ArrowCost = table.Column<byte>(type: "tinyint", nullable: false),
                SenderAction = table.Column<int>(type: "int", nullable: false),
                UpgradeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CastEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CastSound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SenderEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SenderSound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TargetDelay = table.Column<long>(type: "bigint", nullable: false),
                TargetEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TargetSound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                GroundEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TraceEffect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ScreenRepresent = table.Column<long>(type: "bigint", nullable: false),
                UsableInMarket = table.Column<byte>(type: "tinyint", nullable: false),
                TargetWoundDelay = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MagicTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Maps",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DescribeText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DocId = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<long>(type: "bigint", nullable: false),
                Weather = table.Column<long>(type: "bigint", nullable: false),
                BgMusic = table.Column<long>(type: "bigint", nullable: false),
                BgMusicShow = table.Column<long>(type: "bigint", nullable: false),
                PortalX = table.Column<int>(type: "int", nullable: false),
                PortalY = table.Column<int>(type: "int", nullable: false),
                RevivalMapId = table.Column<long>(type: "bigint", nullable: false),
                RevivalPortal = table.Column<long>(type: "bigint", nullable: false),
                Light = table.Column<long>(type: "bigint", nullable: false),
                OwnerId = table.Column<long>(type: "bigint", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                LinkMapId = table.Column<long>(type: "bigint", nullable: false),
                LinkX = table.Column<int>(type: "int", nullable: false),
                LinkY = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Maps", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MonsterTypes",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Type = table.Column<byte>(type: "tinyint", nullable: false),
                AiType = table.Column<byte>(type: "tinyint", nullable: false),
                Look = table.Column<long>(type: "bigint", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                MaxHealth = table.Column<int>(type: "int", nullable: false),
                FleeHealth = table.Column<int>(type: "int", nullable: false),
                Behavior = table.Column<byte>(type: "tinyint", nullable: false),
                SizeAdd = table.Column<byte>(type: "tinyint", nullable: false),
                AttackMin = table.Column<long>(type: "bigint", nullable: false),
                AttackMax = table.Column<long>(type: "bigint", nullable: false),
                Defense = table.Column<long>(type: "bigint", nullable: false),
                Agility = table.Column<byte>(type: "tinyint", nullable: false),
                Dodge = table.Column<byte>(type: "tinyint", nullable: false),
                MagicType = table.Column<long>(type: "bigint", nullable: false),
                MagicDefense = table.Column<long>(type: "bigint", nullable: false),
                MagicHitRate = table.Column<long>(type: "bigint", nullable: false),
                ViewRange = table.Column<byte>(type: "tinyint", nullable: false),
                AttackRange = table.Column<byte>(type: "tinyint", nullable: false),
                AttackSpeed = table.Column<int>(type: "int", nullable: false),
                MoveSpeed = table.Column<int>(type: "int", nullable: false),
                RunSpeed = table.Column<int>(type: "int", nullable: false),
                DropArmet = table.Column<byte>(type: "tinyint", nullable: false),
                DropNecklace = table.Column<byte>(type: "tinyint", nullable: false),
                DropArmor = table.Column<byte>(type: "tinyint", nullable: false),
                DropRing = table.Column<byte>(type: "tinyint", nullable: false),
                DropWeapon = table.Column<byte>(type: "tinyint", nullable: false),
                DropShield = table.Column<byte>(type: "tinyint", nullable: false),
                DropShoes = table.Column<byte>(type: "tinyint", nullable: false),
                DropMoney = table.Column<long>(type: "bigint", nullable: false),
                DropHealthPotion = table.Column<long>(type: "bigint", nullable: false),
                DropManaPotion = table.Column<long>(type: "bigint", nullable: false),
                ExtraExp = table.Column<int>(type: "int", nullable: false),
                ExtraDamage = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MonsterTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Npcs",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Type = table.Column<byte>(type: "tinyint", nullable: false),
                Look = table.Column<long>(type: "bigint", nullable: false),
                Base = table.Column<byte>(type: "tinyint", nullable: false),
                Sort = table.Column<byte>(type: "tinyint", nullable: false),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<int>(type: "int", nullable: false),
                Y = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Npcs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Passways",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                PasswayIdx = table.Column<long>(type: "bigint", nullable: false),
                PortalMapId = table.Column<int>(type: "int", nullable: false),
                PortalIdx = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Passways", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PointAllots",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Profession = table.Column<byte>(type: "tinyint", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Strength = table.Column<int>(type: "int", nullable: false),
                Agility = table.Column<int>(type: "int", nullable: false),
                Vitality = table.Column<int>(type: "int", nullable: false),
                Spirit = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PointAllots", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Portals",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                Idx = table.Column<long>(type: "bigint", nullable: false),
                PortalX = table.Column<int>(type: "int", nullable: false),
                PortalY = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Portals", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Rebirths",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                RequiredProfession = table.Column<byte>(type: "tinyint", nullable: false),
                NewProfession = table.Column<byte>(type: "tinyint", nullable: false),
                RequiredLevel = table.Column<byte>(type: "tinyint", nullable: false),
                NewLevel = table.Column<byte>(type: "tinyint", nullable: false),
                Rebirths = table.Column<byte>(type: "tinyint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rebirths", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Regions",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                Type = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<int>(type: "int", nullable: false),
                Y = table.Column<int>(type: "int", nullable: false),
                Width = table.Column<int>(type: "int", nullable: false),
                Height = table.Column<int>(type: "int", nullable: false),
                DataStr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Data0 = table.Column<long>(type: "bigint", nullable: false),
                Data1 = table.Column<long>(type: "bigint", nullable: false),
                Data2 = table.Column<long>(type: "bigint", nullable: false),
                Data3 = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Regions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ShopItems",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                NpcId = table.Column<long>(type: "bigint", nullable: false),
                ItemTypeId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShopItems", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Traps",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                Look = table.Column<int>(type: "int", nullable: false),
                Data = table.Column<long>(type: "bigint", nullable: false),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<int>(type: "int", nullable: false),
                Y = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Traps", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TrapTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false),
                Sort = table.Column<byte>(type: "tinyint", nullable: false),
                Look = table.Column<long>(type: "bigint", nullable: false),
                ActionId = table.Column<long>(type: "bigint", nullable: false),
                Level = table.Column<long>(type: "bigint", nullable: false),
                AttackMode = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TrapTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WeaponSkillExps",
            columns: table => new
            {
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Experience = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WeaponSkillExps", x => x.Level);
            });

        migrationBuilder.CreateTable(
            name: "GuildAllies",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GuildId = table.Column<long>(type: "bigint", nullable: false),
                AllyGuildId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GuildAllies", x => x.Id);
                table.ForeignKey(
                    name: "FK_GuildAllies_Guilds_GuildId",
                    column: x => x.GuildId,
                    principalTable: "Guilds",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GuildEnemies",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GuildId = table.Column<long>(type: "bigint", nullable: false),
                EnemyGuildId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GuildEnemies", x => x.Id);
                table.ForeignKey(
                    name: "FK_GuildEnemies_Guilds_GuildId",
                    column: x => x.GuildId,
                    principalTable: "Guilds",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Players",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1000001, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                Model = table.Column<long>(type: "bigint", nullable: false),
                Avatar = table.Column<int>(type: "int", nullable: false),
                Hair = table.Column<int>(type: "int", nullable: false),
                Money = table.Column<long>(type: "bigint", nullable: false),
                StorageMoney = table.Column<long>(type: "bigint", nullable: false),
                StoragePin = table.Column<long>(type: "bigint", nullable: false),
                Experience = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                Strength = table.Column<int>(type: "int", nullable: false),
                Agility = table.Column<int>(type: "int", nullable: false),
                Vitality = table.Column<int>(type: "int", nullable: false),
                Spirit = table.Column<int>(type: "int", nullable: false),
                AttributePoints = table.Column<int>(type: "int", nullable: false),
                Virtue = table.Column<long>(type: "bigint", nullable: false),
                Mana = table.Column<int>(type: "int", nullable: false),
                PkMode = table.Column<int>(type: "int", nullable: false),
                PkPoints = table.Column<short>(type: "smallint", nullable: false),
                Profession = table.Column<int>(type: "int", nullable: false),
                Rebirths = table.Column<byte>(type: "tinyint", nullable: false),
                PreviousProfession = table.Column<int>(type: "int", nullable: true),
                Spouse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HomeId = table.Column<long>(type: "bigint", nullable: false),
                Tasks = table.Column<long>(type: "bigint", nullable: false),
                ExperienceBoost = table.Column<int>(type: "int", nullable: false),
                ExperienceBoostTime = table.Column<long>(type: "bigint", nullable: false),
                RebirthBonus = table.Column<int>(type: "int", nullable: false),
                KoRecord = table.Column<int>(type: "int", nullable: false),
                GuildId = table.Column<long>(type: "bigint", nullable: true),
                GuildRank = table.Column<byte>(type: "tinyint", nullable: false),
                GuildDonation = table.Column<long>(type: "bigint", nullable: false),
                RecordMapId = table.Column<long>(type: "bigint", nullable: false),
                RecordX = table.Column<int>(type: "int", nullable: false),
                RecordY = table.Column<int>(type: "int", nullable: false),
                MapId = table.Column<long>(type: "bigint", nullable: false),
                X = table.Column<int>(type: "int", nullable: false),
                Y = table.Column<int>(type: "int", nullable: false),
                Health = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Direction = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Players", x => x.Id);
                table.ForeignKey(
                    name: "FK_Players_Guilds_GuildId",
                    column: x => x.GuildId,
                    principalTable: "Guilds",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Enemies",
            columns: table => new
            {
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                EnemyPlayerId = table.Column<long>(type: "bigint", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enemies", x => new { x.PlayerId, x.EnemyPlayerId });
                table.ForeignKey(
                    name: "FK_Enemies_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Friends",
            columns: table => new
            {
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                FriendPlayerId = table.Column<long>(type: "bigint", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Friends", x => new { x.PlayerId, x.FriendPlayerId });
                table.ForeignKey(
                    name: "FK_Friends_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Items",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false),
                ItemTypeId = table.Column<long>(type: "bigint", nullable: false),
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                Durability = table.Column<int>(type: "int", nullable: false),
                MaxDurability = table.Column<int>(type: "int", nullable: false),
                Ident = table.Column<byte>(type: "tinyint", nullable: false),
                Position = table.Column<byte>(type: "tinyint", nullable: false),
                StorageId = table.Column<long>(type: "bigint", nullable: true),
                Gem1 = table.Column<byte>(type: "tinyint", nullable: false),
                Gem2 = table.Column<byte>(type: "tinyint", nullable: false),
                Magic1 = table.Column<byte>(type: "tinyint", nullable: false),
                Magic2 = table.Column<byte>(type: "tinyint", nullable: false),
                Plus = table.Column<byte>(type: "tinyint", nullable: false),
                Restrain = table.Column<long>(type: "bigint", nullable: false),
                Bless = table.Column<byte>(type: "tinyint", nullable: false),
                Enchant = table.Column<byte>(type: "tinyint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Items", x => x.Id);
                table.ForeignKey(
                    name: "FK_Items_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Magics",
            columns: table => new
            {
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Experience = table.Column<long>(type: "bigint", nullable: false),
                OldLevel = table.Column<byte>(type: "tinyint", nullable: false),
                Unlearned = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Magics", x => new { x.PlayerId, x.Type });
                table.ForeignKey(
                    name: "FK_Magics_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "OfflineMessages",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                Sender = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Words = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OfflineMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_OfflineMessages_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Prizes",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                ItemTypeId = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Prizes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Prizes_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Quests",
            columns: table => new
            {
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                QuestId = table.Column<long>(type: "bigint", nullable: false),
                Slot = table.Column<long>(type: "bigint", nullable: false),
                Value = table.Column<int>(type: "int", nullable: false),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Quests", x => new { x.PlayerId, x.QuestId, x.Slot });
                table.ForeignKey(
                    name: "FK_Quests_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "WeaponSkills",
            columns: table => new
            {
                PlayerId = table.Column<long>(type: "bigint", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<byte>(type: "tinyint", nullable: false),
                Experience = table.Column<long>(type: "bigint", nullable: false),
                OldLevel = table.Column<byte>(type: "tinyint", nullable: false),
                Unlearned = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WeaponSkills", x => new { x.PlayerId, x.Type });
                table.ForeignKey(
                    name: "FK_WeaponSkills_Players_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Players",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GuildAllies_GuildId_AllyGuildId",
            table: "GuildAllies",
            columns: new[] { "GuildId", "AllyGuildId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GuildEnemies_GuildId_EnemyGuildId",
            table: "GuildEnemies",
            columns: new[] { "GuildId", "EnemyGuildId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Guilds_Name",
            table: "Guilds",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Items_PlayerId",
            table: "Items",
            column: "PlayerId");

        migrationBuilder.CreateIndex(
            name: "IX_OfflineMessages_PlayerId",
            table: "OfflineMessages",
            column: "PlayerId");

        migrationBuilder.CreateIndex(
            name: "IX_Players_GuildId",
            table: "Players",
            column: "GuildId");

        migrationBuilder.CreateIndex(
            name: "IX_Players_Name",
            table: "Players",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Players_UserId",
            table: "Players",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Prizes_PlayerId",
            table: "Prizes",
            column: "PlayerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Bounties");

        migrationBuilder.DropTable(
            name: "DynaNpcs");

        migrationBuilder.DropTable(
            name: "Enemies");

        migrationBuilder.DropTable(
            name: "Friends");

        migrationBuilder.DropTable(
            name: "Generators");

        migrationBuilder.DropTable(
            name: "GuildAllies");

        migrationBuilder.DropTable(
            name: "GuildEnemies");

        migrationBuilder.DropTable(
            name: "ItemPluses");

        migrationBuilder.DropTable(
            name: "Items");

        migrationBuilder.DropTable(
            name: "ItemTypes");

        migrationBuilder.DropTable(
            name: "LevelExps");

        migrationBuilder.DropTable(
            name: "Magics");

        migrationBuilder.DropTable(
            name: "MagicTypes");

        migrationBuilder.DropTable(
            name: "Maps");

        migrationBuilder.DropTable(
            name: "MonsterTypes");

        migrationBuilder.DropTable(
            name: "Npcs");

        migrationBuilder.DropTable(
            name: "OfflineMessages");

        migrationBuilder.DropTable(
            name: "Passways");

        migrationBuilder.DropTable(
            name: "PointAllots");

        migrationBuilder.DropTable(
            name: "Portals");

        migrationBuilder.DropTable(
            name: "Prizes");

        migrationBuilder.DropTable(
            name: "Quests");

        migrationBuilder.DropTable(
            name: "Rebirths");

        migrationBuilder.DropTable(
            name: "Regions");

        migrationBuilder.DropTable(
            name: "ShopItems");

        migrationBuilder.DropTable(
            name: "Traps");

        migrationBuilder.DropTable(
            name: "TrapTypes");

        migrationBuilder.DropTable(
            name: "WeaponSkillExps");

        migrationBuilder.DropTable(
            name: "WeaponSkills");

        migrationBuilder.DropTable(
            name: "Players");

        migrationBuilder.DropTable(
            name: "Guilds");
    }
}
