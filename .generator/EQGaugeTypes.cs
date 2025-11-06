enum EQGaugeType {
    None = -1,

    HP = 1, // Gauge0
    Mana = 2, // ManaGauge0
    Stamina = 3,  // STAGauge0
    Fatigue = Stamina,

    Experience = 4,
    AlternateAdvancementExperience = 5,

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
    ExperiencePerHour = 23,
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
    // 

    CombatAbilityWindowTimeRemaining = 26,
    TargetofTargetHP = 27,
    ReSpawnTimer = 28,
    InCombatTimer = 29,
    WebBrowserLoading = 30,

    Group1Mana = 31,
    Group2Mana = 32,
    Group3Mana = 33,
    Group4Mana = 34,
    Group5Mana = 35,

    Group1Endurance = 36,
    Group2Endurance = 37,
    Group3Endurance = 38,
    Group4Endurance = 39,
    Group5Endurance = 40,

    PetsTarget = 41,
}