# MinecraftDiscordStatus
Update a Discord channel to show the number of members online on a Minecraft server.

This update can only happens twice per 10 minutes because of API rate limitations set by Discord.

# Discord
Scope:

- Bot
- application.commands (to create slash commands)

Bot permissions:  

- Manage channels (to update the channel name ofc) 

# Configure your dev environment
- Clone the project
- Rename `appsettings.template.json` to `appsettings.json`
- Complete the configuration file by adding your discord bot token, minecraft ip server, etc
- Build and launch

# Deploy
## Linux

- Publish *MinecraftDiscordStatus* project
- Deploy the files on your linux server
- chmod the *MinecraftDiscordStatus* file to be executable (ideally 0744)
- Execute the *MinecraftDiscordStatus* file (in a "screen" -> `screen -S minecraft` then cd to your deploy folder and finally: `./MinecraftDiscordStatus`, CTRL+A then CTRL+D to detach from your screen session)

# Dependencies
## Minestat
This project uses Minestat to get the player count, as a NuGet package.

URL: https://github.com/FragLand/minestat
