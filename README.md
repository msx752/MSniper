[![All Releases](https://img.shields.io/github/downloads/msx752/MSniper/total.svg?maxAge=100)](https://github.com/msx752/MSniper/releases)
[![Latest Release](https://img.shields.io/github/release/msx752/MSniper.svg?maxAge=100)](https://github.com/msx752/MSniper/releases/latest)
[![GPLv3](https://img.shields.io/badge/license-GPLv3-blue.svg?maxAge=259200)](https://github.com/msx752/MSniper/blob/master/LICENSE.md)
[![Donate](https://img.shields.io/badge/Donate-PayPal-purple.svg)](https://www.paypal.me/mustafasalih)
# MSniper
Manual Snipe for [NecroBot](https://github.com/NoxxDev/NecroBot)

![Display](https://github.com/msx752/MSniper/raw/master/msniper1.gif)


# Description
- MSniper working with `NecroBotv0.9.5 or upper` .You can use everytime because program communicating with NecroBot and it will send pokemon location,so catch doing NecroBot.MSniper can working multiple NecroBot and same pokemon location will send all working bot at the same time.

# Configuration
- `"CatchPokemon": true` of course `TRUE`
- `"Enable": false` set `FALSE` for security  (`It may cause confusion your choice`)
- `"SnipeAtPokestops": false` set `FALSE` we don't need anymore
You don't need another configuring, program will work (`if it TRUE MSniper will now work`).
- `"DisableHumanWalking": true` (`if snipping doesn't work try it but your own risk`)

# File Information
- `registerProtocol.bat:` registers `msniper://` and `pokesniper2://` protocols  (`if you change file location, you have to run again. maybe first you need run removeProtocol.bat`)
- `removeProtocol.bat:` removes msniper:// and  pokesniper2:// protocols
- `resetSnipeList.bat:` resets all working bot's snipping queues

# Protocols
- msniper://
- pokesniper2://

  #### Protocol Example
  - `msniper://Dragonite 12.193245,-120.352751`

## Features

### v1.0.0 > v1.0.2
- integrated with NecroBot (`do not delete SnipeMS.json in NecroBot folder`)
- running without username, password and device information
- Snipping while NecroBot is working
- new protocol `msniper://`
- program working old protocol `pokesiniper2://`
- program can working multiple NecroBot and same pokemon location will send all working NecroBot at the same time

### v1.0.3
- `Settings.json` included
  - `CloseDelaySec`  is duration time
  - `TranslationLanguageCode` everyone know what is this
  - `DownloadNewVersion` new version download checker
- multi-language support
- automatic new version downloader

## Supported Languages
- [Translation List](https://github.com/msx752/MSniper/tree/master/MSniper/Settings/Localization/Languages)

## Advantage
- you can run multiple MSniper
- sending one pokemon, sharing with all running NecroBot
- program can use with PogoLocationFeeder + NecroBot

## Important
- MSniper doesn't cause ban, only using NecroBot will cause ban because every catching is controlling on NecroBot. `using bot your own risk`

## Support
- Frequently asked questions [here](https://github.com/msx752/MSniper/wiki/Frequently-asked-questions-and-solutions)


## Usage
- firstly, run NecroBot (`doesn't matter how many use bot`)
- secondly, run once registerProtocol.bat (`if you change file location, you have to run again`)

  #### Method 1
  - thirdly, use any snipe website

  #### Method 2
  - thirdly, run MSniper.exe normally and paste pokemon location data in console

## Requirements
- [NecroBot](https://github.com/NoxxDev/NecroBot/releases/latest) `v0.9.5 or Upper`

## Download Latest Version
- [MSniper Latest](https://github.com/msx752/MSniper/releases/latest)
