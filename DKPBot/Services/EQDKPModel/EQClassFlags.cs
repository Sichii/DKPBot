using System;

namespace DKPBot.Services.EQDKPModel
{
    [Flags]
    public enum EQClassFlags : ulong
    {
        Mystic = 1,
        Defiler = 1 << 1,
        Shaman = Mystic | Defiler,
        Templar = 1 << 2,
        Inquisitor = 1 << 3,
        Cleric = Templar | Inquisitor,
        Warden = 1 << 4,
        Fury = 1 << 5,
        Druid = Warden | Fury,
        Priest = Shaman | Cleric | Druid,

        Guardian = 1 << 6,
        Berserker = 1 << 7,
        Warrior = Guardian | Berserker,
        Monk = 1 << 8,
        Bruiser = 1 << 9,
        Brawler = Monk | Bruiser,
        Paladin = 1 << 10,
        Shadowknight = 1 << 11,
        Crusader = Paladin | Shadowknight,
        Tank = Warrior | Brawler | Crusader,

        Swashbuckler = 1 << 12,
        Brigand = 1 << 13,
        Rogue = Swashbuckler | Brigand,
        Ranger = 1 << 14,
        Assassin = 1 << 15,
        Predator = Ranger | Assassin,
        Troubador = 1 << 16,
        Dirge = 1 << 17,
        Bard = Troubador | Dirge,
        Scout = Rogue | Predator | Bard,

        Wizard = 1 << 18,
        Warlock = 1 << 19,
        Sorcerer = Wizard | Warlock,
        Illusionist = 1 << 20,
        Coercer = 1 << 21,
        Enchanter = Illusionist | Coercer,
        Conjuror = 1 << 22,
        Necromancer = 1 << 23,
        Summoner = Conjuror | Necromancer,
        Mage = Sorcerer | Enchanter | Summoner
    }
}