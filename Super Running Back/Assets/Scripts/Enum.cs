public enum UIs
{
	None = -1,
	Game,
	Gameover,
	Result,
	MainMenu,
	Stage,
	Option,
	Career
}

public enum Scores
{
	None = -1,
	Time,
	Level,
	Item,
	Hold,
	Kick,
}

public enum GameState
{
	None = -1,
	MainMenu,
	Game,
	Gameover,
	Result,
}

public enum PoolName
{
	Enemy,
	FixedEnemy,
	Item,
	KickParticle,
	KickSound,
	HoldParticle,
	HoldSound,
	LevelUpParticle,
	LevelUpSound,
	PoolNameMax
}

public enum Touchdown
{
	Weak,
	Middle,
	Strong
}

public enum PlayerSound
{
	None = -1,
	Kick,
	Hold,
	Item,
	LevelUp
}