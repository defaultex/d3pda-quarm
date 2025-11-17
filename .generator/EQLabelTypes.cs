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

    PR = SavePoison,
    DR = SaveDisease,
    FR = SaveFire,
    CR = SaveCold,
    MR = SaveMagic,

    CurHP = 17,
    MaxHP = 18,
    HPPercent = 19,
    MPPercent = 20,
    ENPercent = 21,
    
    AC = 22,
    AT = 23,

    CurWeight = 24,
    MaxWeight = 25,
    CurAndMaxWeight = 237,

    XPPercent = 26,
    AAPercent = 27,

    XP = XPPercent,
    AA = AAPercent,

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

    CurAndMaxHP = 70,


    AvailableAA = 71,
    XPtoAA = 72,

    CharacterSurame = 73,
    CharacterTitle = 74,

    CurMP3SongName = 75,
    CurMP3SongDurationMinutesValue = 76,
    CurMP3SongDurationSecondsValue = 77,
    CurMP3SongPositionMinutesValue = 78,
    CurMP3SongPositionSecondsValue = 79,

    PlayersPetName = 68,
    PlayersPetHPPercent = 69,

    // added by zeal
    CurAndMaxMP = 80,
    XPPerHour = 81,
    PetOwner = 82,
    InvSlotsEmpty = 83,
    InvSlotsCount = 84,
    InvSlotsUsed = 85,
    AAPerHour = 86,
    CurMP = 124,
    MaxMP = 125,
    CastingSpellName = 134,
    Song1 = 135, //
    Song2 = 136, //
    Song3 = 137, // 
    Song4 = 138, //
    Song5 = 139, //
    Song6 = 140, //
    //

    CurAndMaxEN = 129,
}