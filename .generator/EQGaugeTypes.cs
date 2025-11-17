enum EQGaugeType {
    None = -1,

    HP = 1, // Gauge0
    Mana = 2, // ManaGauge0
    Stamina = 3,  // STAGauge0
    Fatigue = Stamina,

    XP = 4,
    AA = 5,

    Target = 6,
    Casting = 7,
    Breath = 8,
    Memorize = 9,
    Scribe = 10,

    Group1HP = 11,
    Group2HP = 12,
    Group3HP = 13,
    Group4HP = 14,
    Group5HP = 15,

    PetHP = 16, // PetGauge0
    Group1PetHP = 17,
    Group2PetHP = 18,
    Group3PetHP = 19,
    Group4PetHP = 20,
    Group5PetHP = 21,

    CurrentMP3SongProgress = 22,

    // added by zeal
    XPPerHour = 23,
    ServerTickTimer = 24,
    GlobalRecoveryTimer = 25,
    Spell0Recast = 26, // 
    Spell1Recast = 27, // 
    Spell2Recast = 28, // 
    Spell3Recast = 29, // 
    Spell4Recast = 30, // 
    Spell5Recast = 31, // 
    Spell6Recast = 32, // 
    Spell7Recast = 33, // 
    MeleeRecoveryTimer = 34,
    AAPerHour = 35,
    // zeal aliases
    HPTick = ServerTickTimer,
    ManaTick = ServerTickTimer,
    MeleeTick = MeleeRecoveryTimer,

    //
}