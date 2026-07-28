using EveFitScanUI.Properties;

namespace EveFitScanUI;

internal class ConfigHelper
{
	private static ConfigHelper m_Instance;

	public static ConfigHelper Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new ConfigHelper();
				m_Instance.Load();
			}
			return m_Instance;
		}
	}

	public int WindowPositionX
	{
		get
		{
			return Settings.Default.WindowPositionX;
		}
		set
		{
			Settings.Default.WindowPositionX = value;
			Settings.Default.Save();
		}
	}

	public int WindowPositionY
	{
		get
		{
			return Settings.Default.WindowPositionY;
		}
		set
		{
			Settings.Default.WindowPositionY = value;
			Settings.Default.Save();
		}
	}

	public int WindowWidth
	{
		get
		{
			return Settings.Default.WindowWidth;
		}
		set
		{
			Settings.Default.WindowWidth = value;
			Settings.Default.Save();
		}
	}

	public int WindowHeight
	{
		get
		{
			return Settings.Default.WindowHeight;
		}
		set
		{
			Settings.Default.WindowHeight = value;
			Settings.Default.Save();
		}
	}

	public bool AlwaysOnTop
	{
		get
		{
			return Settings.Default.AlwaysOnTop;
		}
		set
		{
			Settings.Default.AlwaysOnTop = value;
			Settings.Default.Save();
		}
	}

	public bool PassiveTank
	{
		get
		{
			return Settings.Default.PassiveTank;
		}
		set
		{
			Settings.Default.PassiveTank = value;
			Settings.Default.Save();
		}
	}

	public bool STK
	{
		get
		{
			return Settings.Default.STK;
		}
		set
		{
			Settings.Default.STK = value;
			Settings.Default.Save();
		}
	}

	public int SysSecurity
	{
		get
		{
			return Settings.Default.SysSecurity;
		}
		set
		{
			Settings.Default.SysSecurity = value;
			Settings.Default.Save();
		}
	}

	public bool ADCActive
	{
		get
		{
			return Settings.Default.ADCActive;
		}
		set
		{
			Settings.Default.ADCActive = value;
			Settings.Default.Save();
		}
	}

	public bool GetPrices
	{
		get
		{
			return Settings.Default.GetPrices;
		}
		set
		{
			Settings.Default.GetPrices = value;
			Settings.Default.Save();
		}
	}

	public string PriceProvider
	{
		get
		{
			string priceProvider = Settings.Default.PriceProvider;
			return string.IsNullOrEmpty(priceProvider) ? "Goonpraisal" : priceProvider;
		}
		set
		{
			Settings.Default.PriceProvider = value ?? "Goonpraisal";
			Settings.Default.Save();
		}
	}

	public string JaniceApiKey
	{
		get
		{
			return Settings.Default.JaniceApiKey ?? "";
		}
		set
		{
			Settings.Default.JaniceApiKey = value ?? "";
			Settings.Default.Save();
		}
	}

	public bool Highlight
	{
		get
		{
			return Settings.Default.Highlight;
		}
		set
		{
			Settings.Default.Highlight = value;
			Settings.Default.Save();
		}
	}

	public bool ActivateOnFitUpdate
	{
		get
		{
			return Settings.Default.ActivateOnFitUpdate;
		}
		set
		{
			Settings.Default.ActivateOnFitUpdate = value;
			Settings.Default.Save();
		}
	}

	public int Manual_EHP
	{
		get
		{
			return Settings.Default.Manual_EHP;
		}
		set
		{
			Settings.Default.Manual_EHP = value;
			Settings.Default.Save();
		}
	}

	public bool Is_Manual_EHP
	{
		get
		{
			return Settings.Default.Is_Manual_EHP;
		}
		set
		{
			Settings.Default.Is_Manual_EHP = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Mjolnir
	{
		get
		{
			return Settings.Default.DPS_Mjolnir;
		}
		set
		{
			Settings.Default.DPS_Mjolnir = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Nova
	{
		get
		{
			return Settings.Default.DPS_Nova;
		}
		set
		{
			Settings.Default.DPS_Nova = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Antimatter
	{
		get
		{
			return Settings.Default.DPS_Antimatter;
		}
		set
		{
			Settings.Default.DPS_Antimatter = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Void
	{
		get
		{
			return Settings.Default.DPS_Void;
		}
		set
		{
			Settings.Default.DPS_Void = value;
			Settings.Default.Save();
		}
	}

	public int DPS_VoidL
	{
		get
		{
			return Settings.Default.DPS_VoidL;
		}
		set
		{
			Settings.Default.DPS_VoidL = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Multifrequency
	{
		get
		{
			return Settings.Default.DPS_Multifrequency;
		}
		set
		{
			Settings.Default.DPS_Multifrequency = value;
			Settings.Default.Save();
		}
	}

	public int DPS_EMP
	{
		get
		{
			return Settings.Default.DPS_EMP;
		}
		set
		{
			Settings.Default.DPS_EMP = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Phased_Plasma
	{
		get
		{
			return Settings.Default.DPS_Phased_Plasma;
		}
		set
		{
			Settings.Default.DPS_Phased_Plasma = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Fusion
	{
		get
		{
			return Settings.Default.DPS_Fusion;
		}
		set
		{
			Settings.Default.DPS_Fusion = value;
			Settings.Default.Save();
		}
	}

	public int DPS_Hail
	{
		get
		{
			return Settings.Default.DPS_Hail;
		}
		set
		{
			Settings.Default.DPS_Hail = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Mjolnir
	{
		get
		{
			return Settings.Default.RoF_Mjolnir;
		}
		set
		{
			Settings.Default.RoF_Mjolnir = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Nova
	{
		get
		{
			return Settings.Default.RoF_Nova;
		}
		set
		{
			Settings.Default.RoF_Nova = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Antimatter
	{
		get
		{
			return Settings.Default.RoF_Antimatter;
		}
		set
		{
			Settings.Default.RoF_Antimatter = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Void
	{
		get
		{
			return Settings.Default.RoF_Void;
		}
		set
		{
			Settings.Default.RoF_Void = value;
			Settings.Default.Save();
		}
	}

	public double RoF_VoidL
	{
		get
		{
			return Settings.Default.RoF_VoidL;
		}
		set
		{
			Settings.Default.RoF_VoidL = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Multifrequency
	{
		get
		{
			return Settings.Default.RoF_Multifrequency;
		}
		set
		{
			Settings.Default.RoF_Multifrequency = value;
			Settings.Default.Save();
		}
	}

	public double RoF_EMP
	{
		get
		{
			return Settings.Default.RoF_EMP;
		}
		set
		{
			Settings.Default.RoF_EMP = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Phased_Plasma
	{
		get
		{
			return Settings.Default.RoF_Phased_Plasma;
		}
		set
		{
			Settings.Default.RoF_Phased_Plasma = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Fusion
	{
		get
		{
			return Settings.Default.RoF_Fusion;
		}
		set
		{
			Settings.Default.RoF_Fusion = value;
			Settings.Default.Save();
		}
	}

	public double RoF_Hail
	{
		get
		{
			return Settings.Default.RoF_Hail;
		}
		set
		{
			Settings.Default.RoF_Hail = value;
			Settings.Default.Save();
		}
	}

	public string PassiveColdHot
	{
		get
		{
			return Settings.Default.PassiveColdHot;
		}
		set
		{
			Settings.Default.PassiveColdHot = value;
			Settings.Default.Save();
		}
	}

	private void Load()
	{
	}

	private ConfigHelper()
	{
	}

	public void ResetDpsRoF()
	{
		DPS_Mjolnir = 1019;
		RoF_Mjolnir = 6.26;
		DPS_Nova = 1019;
		RoF_Nova = 6.26;
		DPS_Antimatter = 448;
		RoF_Antimatter = 1.91;
		DPS_Void = 818;
		RoF_Void = 1.87;
		DPS_VoidL = 1773;
		RoF_VoidL = 4.16;
		DPS_Multifrequency = 312;
		RoF_Multifrequency = 2.4;
		DPS_EMP = 387;
		RoF_EMP = 2.11;
		DPS_Fusion = 387;
		RoF_Fusion = 2.11;
		DPS_Phased_Plasma = 387;
		RoF_Phased_Plasma = 2.11;
		DPS_Hail = 596;
		RoF_Hail = 2.07;
	}

	public void RepairDpsRoF()
	{
		if (DPS_Mjolnir == 0)
		{
			DPS_Mjolnir = 846;
		}
		if (RoF_Mjolnir == 0.0)
		{
			RoF_Mjolnir = 6.77;
		}
		if (DPS_Nova == 0)
		{
			DPS_Nova = 846;
		}
		if (RoF_Nova == 0.0)
		{
			RoF_Nova = 6.77;
		}
		if (DPS_Antimatter == 0)
		{
			DPS_Antimatter = 390;
		}
		if (RoF_Antimatter == 0.0)
		{
			RoF_Antimatter = 2.05;
		}
		if (DPS_Void == 0)
		{
			DPS_Void = 731;
		}
		if (RoF_Void == 0.0)
		{
			RoF_Void = 1.96;
		}
		if (DPS_VoidL == 0)
		{
			DPS_VoidL = 1521;
		}
		if (RoF_VoidL == 0.0)
		{
			RoF_VoidL = 4.37;
		}
		if (DPS_Multifrequency == 0)
		{
			DPS_Multifrequency = 272;
		}
		if (RoF_Multifrequency == 0.0)
		{
			RoF_Multifrequency = 2.58;
		}
		if (DPS_EMP == 0)
		{
			DPS_EMP = 331;
		}
		if (RoF_EMP == 0.0)
		{
			RoF_EMP = 2.21;
		}
		if (DPS_Fusion == 0)
		{
			DPS_Fusion = 331;
		}
		if (RoF_Fusion == 0.0)
		{
			RoF_Fusion = 2.21;
		}
		if (DPS_Phased_Plasma == 0)
		{
			DPS_Phased_Plasma = 331;
		}
		if (RoF_Phased_Plasma == 0.0)
		{
			RoF_Phased_Plasma = 2.21;
		}
		if (DPS_Hail == 0)
		{
			DPS_Hail = 511;
		}
		if (RoF_Hail == 0.0)
		{
			RoF_Hail = 2.17;
		}
	}

	public void ResetDpsRoFScrub()
	{
		DPS_Mjolnir = 0;
		DPS_Nova = 0;
		DPS_Antimatter = 0;
		DPS_Void = 0;
		DPS_VoidL = 0;
		DPS_Multifrequency = 0;
		DPS_EMP = 0;
		DPS_Fusion = 0;
		DPS_Phased_Plasma = 0;
		DPS_Hail = 0;
		RoF_Mjolnir = 0.0;
		RoF_Nova = 0.0;
		RoF_Antimatter = 0.0;
		RoF_Void = 0.0;
		RoF_VoidL = 0.0;
		RoF_Multifrequency = 0.0;
		RoF_EMP = 0.0;
		RoF_Fusion = 0.0;
		RoF_Phased_Plasma = 0.0;
		RoF_Hail = 0.0;
		RepairDpsRoF();
	}
}
