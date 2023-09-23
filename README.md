# BunnyCraft
------------
Update a channel to show the number of members online on our FC Minecraft server.

Invite link: https://discord.com/api/oauth2/authorize?client_id=1155156935209926736&permissions=67108880&scope=bot

# Discord
---------

Scope:
	- Bot

Bot permissions:
	- Manage channels
	- Change nickname

# Minestat dependency
---------------------
https://github.com/FragLand/minestat

# Deploy
--------

## Linux

- Publish *MinecraftDiscordStatus* project
- Deploy the files on your linux server
- chmod the *MinecraftDiscordStatus* file to be executable (ideally 0744)
- Execute the *MinecraftDiscordStatus* file (in a "screen" -> `screen -S minecraft` then cd to your deploy folder and finally: `./MinecraftDiscordStatus`, CTRL+A then CTRL+D to detach from your screen session)