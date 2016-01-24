using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager 
{
	NetworkManager manager;
	public GameObject mainPanel;
	public GameObject matckmakerPanel;
	public GameObject matchBrowserPanel;

	void Start()		//must use start because we don't want to override Awake()!
	{
		manager = GetComponent<NetworkManager>();
		matckmakerPanel.SetActive(false);
		matchBrowserPanel.SetActive(false);
	}

	public void StartupHost()
	{
		SetPort();
		NetworkManager.singleton.StartHost();
	}

	public void JoinGame()
	{
		SetIPAddress();
		SetPort();
		NetworkManager.singleton.StartClient();
	}

	public void EnableMatchmaker()
	{
		manager.StartMatchMaker();
		mainPanel.SetActive(false);
		matckmakerPanel.SetActive(true);
	}

	public void DisableMatchmaker()
	{
		manager.StopMatchMaker();
		matckmakerPanel.SetActive(false);
		matchBrowserPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

	void SetIPAddress()
	{
		string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	public void CreateMatch()
	{
		SetMatchName();
		manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
	}

	void SetMatchName()
	{
		string matchName = GameObject.Find("InputFieldMatchName").transform.FindChild("Text").GetComponent<Text>().text;
		manager.matchName = matchName;
	}

	void OnLevelWasLoaded(int level)
	{
		if (level == 0)
		{
			//SetupMenuSceneButtons();
			StartCoroutine(SetupMenuSceneButtons());
		}
		else
		{
			SetupOtherSceneButtons();
		}
	}

	IEnumerator SetupMenuSceneButtons()
	{
		yield return new WaitForSeconds(0.1f);

		mainPanel = GameObject.Find("PanelMain");
		matckmakerPanel = GameObject.Find("PanelMatchmaking");
		matchBrowserPanel = GameObject.Find("PanelMatchBrowser");

		matckmakerPanel.SetActive(false);
		matchBrowserPanel.SetActive(false);

		GameObject.Find("ButtonEnableMatchmaker").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonEnableMatchmaker").GetComponent<Button>().onClick.AddListener(EnableMatchmaker);

		GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

		GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
	}

	void SetupOtherSceneButtons()
	{
		GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
	}
}
