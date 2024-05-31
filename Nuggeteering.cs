using Godot;
using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace GritClicker
{
	public partial class Nuggeteering : Node2D
	{
		private string name;
		private int level = 1;
		private long money = 0;
		private int upgradeCost = 0;
		private int moneyMultiplier = 1;
		private string textureLocation;
		private Label levelLabel;
		private Label moneyLabel;
		private TextureButton nuggetButton;
		private Button upgradeButton;
		internal const string connectionString = "Data Source=./mp3.db;";
		internal static SqliteConnection connection = new(connectionString);

		public override void _Ready()
		{
			levelLabel = GetNode<Label>("NuggetLevel");
			moneyLabel = GetNode<Label>("NuggetScore");
			nuggetButton = GetNode<TextureButton>("Nugget");
			nuggetButton.Pressed += OnClick;
			upgradeButton = GetNode<Button>("UpgradeButton");
			upgradeButton.Pressed += OnUpgrade;
			List<string> data = DataParseOnStart(connection);
			name = data[0];
			level = Int32.Parse(data[1]);
			money = Int32.Parse(data[2]);
			levelLabel.Text = String.Format("Level {0}", level);
		}

		public void OnClick()
		{
			money += moneyMultiplier;
			moneyLabel.Text = String.Format("${0}", money.ToString());
		}

		public void OnUpgrade()
		{
			if (money > upgradeCost)
			{
				money -= upgradeCost;
				moneyMultiplier++;
				level++;
				levelLabel.Text = String.Format("Level {0}", level);
				UserCreation.UpgradeUser(new User(name, level, money), "up");
				UpgradeFromDB();
			}
			else
			{
				moneyLabel.Text = "Not enough money!";
			}
		}

		public override void _Notification(int what)
		{
			if (what == NotificationWMCloseRequest){
				UserCreation.UpgradeUser(new User(name, level, money), "exit");
			}
		}

		public static List<string> DataParseOnStart(SqliteConnection connection)
		{
			DBInitializing.Init(connection);
			List<string> data = DBInitializing.GameDataRetrieve(connection);
			return data;
		}

		public void UpgradeFromDB()
		{
			List<string> data = DataParseOnStart(connection);
			upgradeCost = Int32.Parse(data[4]);
			textureLocation = data[5];
			nuggetButton.TextureNormal = (Texture2D)GD.Load(textureLocation);
		}
	}
}
