enum EQLabelType {
    None = -1,

    Name = 1,
    Level = 2,
    Class = 3,
    Diety = 4,

    STR = 5,
    STA = 6,
    DEX = 7,
    AGI = 8,
    WIS = 9,
    INT = 10,
    CHA = 11,

    SavePoison = 12,
    SaveDisease = 13,
    SaveFire = 14,
    SaveCold = 15,
    SaveMagic = 16,

    HPPercent = 19,
    CurHP = 17,
    MaxHP = 18,
    CurAndMaxHP = 70,

    MPPercent = 20,
    CurMP = 124,
    MaxMP = 125,
    CurAndMaxMP = 80, //128,

    ENPercent = 21,
    CurEN = 126,
    MaxEN = 127,
    CurAndMaxEN = 129,

    CurMIT = 22,
    CutOFF = 23,

    CurWeight = 24,
    MaxWeight = 25,
    CurAndMaxWeight = 237,

    XPPercent = 26,
    AAPercent = 27,

    TargetName = 28,
    TargetHPPercent = 29,

    GroupMember1Name = 30,
    GroupMember2Name = 31,
    GroupMember3Name = 32,
    GroupMember4Name = 33,
    GroupMember5Name = 34,

    GroupMember1HPPercent = 35,
    GroupMember2HPPercent = 36,
    GroupMember3HPPercent = 37,
    GroupMember4HPPercent = 38,
    GroupMember5HPPercent = 39,

    GroupPet1HPPercent = 40,
    GroupPet2HPPercent = 41,
    GroupPet3HPPercent = 42,
    GroupPet4HPPercent = 43,
    GroupPet5HPPercent = 44,

    OldBuff0 = 45,
    OldBuff1 = 46,
    OldBuff2 = 47,
    OldBuff3 = 48,
    OldBuff4 = 49,
    OldBuff5 = 50,
    OldBuff6 = 51,
    OldBuff7 = 52,
    OldBuff8 = 53,
    OldBuff9 = 54,
    OldBuff10 = 55,
    OldBuff11 = 56,
    OldBuff12 = 57,
    OldBuff13 = 58,
    OldBuff14 = 59,

    Spell0 = 60,
    Spell1 = 61,
    Spell2 = 62,
    Spell3 = 63,
    Spell4 = 64,
    Spell5 = 65,
    Spell6 = 66,
    Spell7 = 67,
    Spell8 = 133,
    Spell9 = 138,
    Spell10 = 149,
    Spell11 = 150,
    Spell12 = 414,
    Spell13 = 415,
    CastingSpellName = 134,

    AvailableAA = 71,
    XPtoAA = 72,

    CharacterSurame = 73,
    CharacterTitle = 74,

    CurMP3SongName = 75,
    CurMP3SongDurationMinutesValue = 76,
    CurMP3SongDurationSecondsValue = 77,
    CurMP3SongPositionMinutesValue = 78,
    CurMP3SongPositionSecondsValue = 79,

    Song1 = 80,
    Song2 = 81,
    Song3 = 82,
    Song4 = 83,
    Song5 = 84,
    Song6 = 85,

    PlayersPetName = 68,
    PlayersPetHPPercent = 69,

    PetBuff1 = 86,
    PetBuff2 = 87,
    PetBuff3 = 88,
    PetBuff4 = 89,
    PetBuff5 = 90,
    PetBuff6 = 91,
    PetBuff7 = 92,
    PetBuff8 = 93,
    PetBuff9 = 94,
    PetBuff10 = 95,
    PetBuff11 = 96,
    PetBuff12 = 97,
    PetBuff13 = 98,
    PetBuff14 = 99,
    PetBuff15 = 100,
    PetBuff16 = 101,
    PetBuff17 = 102,
    PetBuff18 = 103,
    PetBuff19 = 104,
    PetBuff20 = 105,
    PetBuff21 = 106,
    PetBuff22 = 107,
    PetBuff23 = 108,
    PetBuff24 = 109,
    PetBuff25 = 110,
    PetBuff26 = 111,
    PetBuff27 = 112,
    PetBuff28 = 113,
    PetBuff29 = 114,
    PetBuff30 = 115,

    PersonalTributeTimer = 116,
    CurTributePoints = 117,
    TotalCareerTribute = 118,
    TributeCostPer10Mins = 119,

    TargetofTargetPercent = 120,

    GuildTributeTimer = 121,
    GuildTributePool = 122,
    GuildTributePayment = 123,

    TaskSystemDurationTimer = 132,

    TargetofTargetName = 135,

    PlayerCombatTimerLabel = 137,

    GroupMember1ManaPercentage = 139,
    GroupMember2ManaPercentage = 140,
    GroupMember3ManaPercentage = 141,
    GroupMember4ManaPercentage = 142,
    GroupMember5ManaPercentage = 143,

    GroupMember1EndurancePercentage = 144,
    GroupMember2EndurancePercentage = 145,
    GroupMember3EndurancePercentage = 146,
    GroupMember4EndurancePercentage = 147,
    GroupMember5EndurancePercentage = 148,

    HPPercentageExtendedTargetWindow0 = 151,
    HPPercentageExtendedTargetWindow1 = 152,
    HPPercentageExtendedTargetWindow2 = 153,
    HPPercentageExtendedTargetWindow3 = 154,
    HPPercentageExtendedTargetWindow4 = 155,
    HPPercentageExtendedTargetWindow5 = 156,
    HPPercentageExtendedTargetWindow6 = 157,
    HPPercentageExtendedTargetWindow7 = 158,
    HPPercentageExtendedTargetWindow8 = 159,
    HPPercentageExtendedTargetWindow9 = 160,
    HPPercentageExtendedTargetWindow10 = 161,
    HPPercentageExtendedTargetWindow11 = 162,
    HPPercentageExtendedTargetWindow12 = 163,
    HPPercentageExtendedTargetWindow13 = 164,
    HPPercentageExtendedTargetWindow14 = 165,
    HPPercentageExtendedTargetWindow15 = 166,
    HPPercentageExtendedTargetWindow16 = 167,
    HPPercentageExtendedTargetWindow17 = 168,
    HPPercentageExtendedTargetWindow18 = 169,
    HPPercentageExtendedTargetWindow19 = 170,

    ManaPercentageExtendedTargetWindow0 = 171,
    ManaPercentageExtendedTargetWindow1 = 172,
    ManaPercentageExtendedTargetWindow2 = 173,
    ManaPercentageExtendedTargetWindow3 = 174,
    ManaPercentageExtendedTargetWindow4 = 175,
    ManaPercentageExtendedTargetWindow5 = 176,
    ManaPercentageExtendedTargetWindow6 = 177,
    ManaPercentageExtendedTargetWindow7 = 178,
    ManaPercentageExtendedTargetWindow8 = 179,
    ManaPercentageExtendedTargetWindow9 = 180,
    ManaPercentageExtendedTargetWindow10 = 181,
    ManaPercentageExtendedTargetWindow11 = 182,
    ManaPercentageExtendedTargetWindow12 = 183,
    ManaPercentageExtendedTargetWindow13 = 184,
    ManaPercentageExtendedTargetWindow14 = 185,
    ManaPercentageExtendedTargetWindow15 = 186,
    ManaPercentageExtendedTargetWindow16 = 187,
    ManaPercentageExtendedTargetWindow17 = 188,
    ManaPercentageExtendedTargetWindow18 = 189,
    ManaPercentageExtendedTargetWindow19 = 190,

    EndurancePercentageExtendedTargetWindow0 = 191,
    EndurancePercentageExtendedTargetWindow1 = 192,
    EndurancePercentageExtendedTargetWindow2 = 193,
    EndurancePercentageExtendedTargetWindow3 = 194,
    EndurancePercentageExtendedTargetWindow4 = 195,
    EndurancePercentageExtendedTargetWindow5 = 196,
    EndurancePercentageExtendedTargetWindow6 = 197,
    EndurancePercentageExtendedTargetWindow7 = 198,
    EndurancePercentageExtendedTargetWindow8 = 199,
    EndurancePercentageExtendedTargetWindow9 = 200,
    EndurancePercentageExtendedTargetWindow10 = 201,
    EndurancePercentageExtendedTargetWindow11 = 202,
    EndurancePercentageExtendedTargetWindow12 = 203,
    EndurancePercentageExtendedTargetWindow13 = 204,
    EndurancePercentageExtendedTargetWindow14 = 205,
    EndurancePercentageExtendedTargetWindow15 = 206,
    EndurancePercentageExtendedTargetWindow16 = 207,
    EndurancePercentageExtendedTargetWindow17 = 208,
    EndurancePercentageExtendedTargetWindow18 = 209,
    EndurancePercentageExtendedTargetWindow19 = 210,

    Haste = 211,
    HitPointRegeneration = 212,
    ManaRegeneration = 213,
    EnduranceRegeneration = 214,
    SpellShield = 215,
    CombatEffects = 216,
    Shielding = 217,
    DamageShielding = 218,
    DamageOverTimeShielding = 219,
    DamageShieldMitigation = 220,
    Avoidance = 221,
    Accuracy = 222,
    StunResist = 223,
    StrikeThrough = 224,
    HealAmount = 225,
    SpellDamage = 226,
    Clairvoyance = 227,

    SkillDamageBash = 228,
    SkillDamageBackstab = 229,
    SkillDamageDragonpunch = 230,
    SkillDamageEaglestrike = 231,
    SkillDamageFlyingkick = 232,
    SkillDamageKick = 233,
    SkillDamageRoundkick = 234,
    SkillDamageTigerclaw = 235,
    SkillDamageFrenzy = 236,

    BaseStrength = 238,
    BaseStamina = 239,
    BaseDexterity = 240,
    BaseAgility = 241,
    BaseWisdom = 242,
    BaseIntelligence = 243,
    BaseCharisma = 244,
    BaseSavePoison = 245,
    BaseSaveDisease = 246,
    BaseSaveFire = 247,
    BaseSaveCold = 248,
    BaseSaveMagic = 249,
    BaseSaveCorruption = 250,

    HeroicStrength = 251,
    HeroicStamina = 252,
    HeroicDexterity = 253,
    HeroicAgility = 254,
    HeroicWisdom = 255,
    HeroicIntelligence = 256,
    HeroicCharisma = 257,
    HeroicSavePoison = 258,
    HeroicSaveDisease = 259,
    HeroicSaveFire = 260,
    HeroicSaveCold = 261,
    HeroicSaveMagic = 262,
    HeroicSaveCorruption = 263,

    CapStrength = 264,
    CapStamina = 265,
    CapDexterity = 266,
    CapAgility = 267,
    CapWisdom = 268,
    CapIntelligence = 269,
    CapCharisma = 270,
    CapSavePoison = 271,
    CapSaveDisease = 272,
    CapSaveFire = 273,
    CapSaveCold = 274,
    CapSaveMagic = 275,
    CapSaveCorruption = 276,
    CapSpellShield = 277,
    CapCombatEffects = 278,
    CapShielding = 279,
    CapDamageShielding = 280,
    CapDamageOverTimeShielding = 281,
    CapDamageShieldMitigation = 282,
    CapAvoidance = 283,
    CapAccuracy = 284,
    CapStunResist = 285,
    CapStrikeThrough = 286,
    CapSkillDamageBash = 287,
    CapSkillDamageBackstab = 288,
    CapSkillDamageDragonpunch = 289,
    CapSkillDamageEaglestrike = 290,
    CapSkillDamageFlyingkick = 291,
    CapSkillDamageKick = 292,
    CapSkillDamageRoundkick = 293,
    CapSkillDamageTigerclaw = 294,
    CapSkillDamageFrenzy = 295,

    LoyaltyTokenCount = 296,
    TributeTrophyTimer = 297,
    TributeTrophyCost = 298,
    GuildTributeTrophyTimer = 299,
    GuildTributeTrophyCost = 300,

    TargetofPetHP = 301,

    AggroTargetName = 302,

    AggroMostHatedName = 303,
    AggroMostHatedNameNoLock = 304,

    AggroMyHatePercent = 305,
    AggroMyHatePercentNoLock = 306,

    AggroMostHatedHatePercent = 307,
    AggroMostHatedHatePercentNoLock = 308,

    AggroGroup1HatePercent = 309,
    AggroGroup2HatePercent = 310,
    AggroGroup3HatePercent = 311,
    AggroGroup4HatePercent = 312,
    AggroGroup5HatePercent = 313,

    AggroExtendedTarget0HatePercent = 314,
    AggroExtendedTarget1HatePercent = 315,
    AggroExtendedTarget2HatePercent = 316,
    AggroExtendedTarget3HatePercent = 317,
    AggroExtendedTarget4HatePercent = 318,
    AggroExtendedTarget5HatePercent = 319,
    AggroExtendedTarget6HatePercent = 320,
    AggroExtendedTarget7HatePercent = 321,
    AggroExtendedTarget8HatePercent = 322,
    AggroExtendedTarget9HatePercent = 323,
    AggroExtendedTarget10HatePercent = 324,
    AggroExtendedTarget11HatePercent = 325,
    AggroExtendedTarget12HatePercent = 326,
    AggroExtendedTarget13HatePercent = 327,
    AggroExtendedTarget14HatePercent = 328,
    AggroExtendedTarget15HatePercent = 329,
    AggroExtendedTarget16HatePercent = 330,
    AggroExtendedTarget17HatePercent = 331,
    AggroExtendedTarget18HatePercent = 332,
    AggroExtendedTarget19HatePercent = 333,

    MercenaryAAExperiencePercentLabel = 335,
    MercenaryAAExperiencePointsLabel = 336,
    MercenaryAAExperiencePointsSpentLabel = 337,
    MercenaryHP = 338,
    MercenaryMaxHP = 339,
    MercenaryMana = 340,
    MercenaryMaxMana = 341,
    MercenaryEndurance = 342,
    MercenaryMaxEndurance = 343,
    MercenaryArmorClass = 344,
    MercenaryAttack = 345,
    MercenaryHastePercent = 346,
    MercenaryStrength = 347,
    MercenaryStamina = 348,
    MercenaryIntelligence = 349,
    MercenaryWisdom = 350,
    MercenaryAgility = 351,
    MercenaryDexterity = 352,
    MercenaryCharisma = 353,
    MercenaryCombatHPRegeneration = 354,
    MercenaryCombatManaRegeneration = 355,
    MercenaryCombatEnduranceRegeneration = 356,
    MercenaryHealAmount = 357,
    MercenarySpellDamage = 358,

    PowerSourcePercentageRemaining = 360,

    Velocity = 401,
    AccuracyTooHit = 402,
    Evasion = 403,

    Buff0 = 500,
    Buff1 = 501,
    Buff2 = 502,
    Buff3 = 503,
    Buff4 = 504,
    Buff5 = 505,
    Buff6 = 506,
    Buff7 = 507,
    Buff8 = 508,
    Buff9 = 509,
    Buff10 = 510,
    Buff11 = 511,
    Buff12 = 512,
    Buff13 = 513,
    Buff14 = 514,
    Buff15 = 515,
    Buff16 = 516,
    Buff17 = 517,
    Buff18 = 518,
    Buff19 = 519,
    Buff20 = 520,
    Buff21 = 521,
    Buff22 = 522,
    Buff23 = 523,
    Buff24 = 524,
    Buff25 = 525,
    Buff26 = 526,
    Buff27 = 527,
    Buff28 = 528,
    Buff29 = 529,
    Buff30 = 530,
    Buff31 = 531,
    Buff32 = 532,
    Buff33 = 533,
    Buff34 = 534,
    Buff35 = 535,
    Buff36 = 536,
    Buff37 = 537,
    Buff38 = 538,
    Buff39 = 539,
    Buff40 = 540,
    Buff41 = 541,

    BlockedBuff0 = 550,
    BlockedBuff1 = 551,
    BlockedBuff2 = 552,
    BlockedBuff3 = 553,
    BlockedBuff4 = 554,
    BlockedBuff5 = 555,
    BlockedBuff6 = 556,
    BlockedBuff7 = 557,
    BlockedBuff8 = 558,
    BlockedBuff9 = 559,
    BlockedBuff10 = 560,
    BlockedBuff11 = 561,
    BlockedBuff12 = 562,
    BlockedBuff13 = 563,
    BlockedBuff14 = 564,
    BlockedBuff15 = 565,
    BlockedBuff16 = 566,
    BlockedBuff17 = 567,
    BlockedBuff18 = 568,
    BlockedBuff19 = 569,
    BlockedBuff20 = 570,
    BlockedBuff21 = 571,
    BlockedBuff22 = 572,
    BlockedBuff23 = 573,
    BlockedBuff24 = 574,
    BlockedBuff25 = 575,
    BlockedBuff26 = 576,
    BlockedBuff27 = 577,
    BlockedBuff28 = 578,
    BlockedBuff29 = 579,

    SongBuff0 = 600,
    SongBuff1 = 601,
    SongBuff2 = 602,
    SongBuff3 = 603,
    SongBuff4 = 604,
    SongBuff5 = 605,
    SongBuff6 = 606,
    SongBuff7 = 607,
    SongBuff8 = 608,
    SongBuff9 = 609,
    SongBuff10 = 610,
    SongBuff11 = 611,
    SongBuff12 = 612,
    SongBuff13 = 613,
    SongBuff14 = 614,
    SongBuff15 = 615,
    SongBuff16 = 616,
    SongBuff17 = 617,
    SongBuff18 = 618,
    SongBuff19 = 619,
    SongBuff20 = 620,
    SongBuff21 = 621,
    SongBuff22 = 622,
    SongBuff23 = 623,
    SongBuff24 = 624,
    SongBuff25 = 625,
    SongBuff26 = 626,
    SongBuff27 = 627,
    SongBuff28 = 628,
    SongBuff29 = 629,

    PetBlockedBuff0 = 650,
    PetBlockedBuff1 = 651,
    PetBlockedBuff2 = 652,
    PetBlockedBuff3 = 653,
    PetBlockedBuff4 = 654,
    PetBlockedBuff5 = 655,
    PetBlockedBuff6 = 656,
    PetBlockedBuff7 = 657,
    PetBlockedBuff8 = 658,
    PetBlockedBuff9 = 659,
    PetBlockedBuff10 = 660,
    PetBlockedBuff11 = 661,
    PetBlockedBuff12 = 662,
    PetBlockedBuff13 = 663,
    PetBlockedBuff14 = 664,
    PetBlockedBuff15 = 665,
    PetBlockedBuff16 = 666,
    PetBlockedBuff17 = 667,
    PetBlockedBuff18 = 668,
    PetBlockedBuff19 = 669,
    PetBlockedBuff20 = 670,
    PetBlockedBuff21 = 671,
    PetBlockedBuff22 = 672,
    PetBlockedBuff23 = 673,
    PetBlockedBuff24 = 674,
    PetBlockedBuff25 = 675,
    PetBlockedBuff26 = 676,
    PetBlockedBuff27 = 677,
    PetBlockedBuff28 = 678,
    PetBlockedBuff29 = 679
}