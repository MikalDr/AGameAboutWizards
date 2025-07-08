using Godot;
using System;

public partial class MultiPlrTestScene : Node2D
{
  private ENetMultiplayerPeer _peer = new();
  [Export] private PackedScene _playerScene;
  [Export] private Button _hostBtn;
  [Export] private Button _joinBtn;
  [Export] private int _port = 8080;
  [Export] private string _address = "127.0.0.1";
  [Export] private int _maxClientCount = 4;

  public override void _Ready()
  {
	_hostBtn.ButtonUp += () =>
	{
	  var err = _peer.CreateServer(_port, _maxClientCount);
	  if (err != Error.Ok)
		  throw new Exception("Failed to create server: ${err.ToString()}");
	  Multiplayer.MultiplayerPeer = _peer;
	  Multiplayer.PeerConnected += _AddPlr;
	  _AddPlr(1);
	};

	_joinBtn.ButtonUp += () =>
	{
	  var err = _peer.CreateClient(_address, _port);
	  if (err != Error.Ok)
			throw new Exception("Failed to create server: ${err.ToString()}");
	  Multiplayer.MultiplayerPeer = _peer;
	};
  }

  private void _AddPlr(long id)
  {
	var plr = _playerScene.Instantiate();
	plr.Name = id.ToString();
	CallDeferred("add_child", plr);   
  }
}
