# DSpico Launcher Theme Converter
This project is a Windows-only supported tool to convert themes to the DSpico Theme format, a open-source launcher for the Nintendo DS. This tool is still in development and may not work perfectly for all themes.

It allows users to convert themes from various themes launcher to the theme for DSpico launcher. 


However it is important to note that the tool may not be able to convert all themes perfectly, as some themes may have unique features or design elements that may not be compatible with the DSpico Theme format.


Some themes may require manual adjustments after conversion to work properly on the DSpico Theme.


Also, please note that this tool is not affiliated with the developers of the DSpico Launcher or any other launchers mentioned in this project. 
It is an independent project created by me, Djinnet, for the purpose of converting themes for the DSpico Launcher.


# Metadata info:
The tool will generate metadata info (such as the theme name, the author name, the description, etc.) from the original theme, but it is not guaranteed that all the metadata info will be correct info during the conversion process. 


Some themes may have metadata info that is not compatible with the DSpico theme format, and in such cases, the tool may not be able to preserve all the metadata info. 


Therefore, it is recommended to check the converted theme's metadata info after conversion and make any necessary adjustments manually if needed.


However, if you are using metadata.ini file for your themes, the tool will ensure to preserve all the metadata info from the original theme, as long as the metadata.ini file is properly formatted and contains all the necessary information.

Also you can preview the metadata info in the UI of the tool before conversion, so you can customize/overwrite the metadata info for your themes as well.


# How to use the program:
1. First prepare your source theme in term of metadata and theme structure.
2. Browse in UI after the source theme. Most times the tool will be able to automatically detect the theme type and display it in the UI, but if the tool is not able to detect the theme type, you can manually pick the theme in the UI dropdown.
3. Review if the UI previewer looks correct. Overwrite the theme metadata if needed.
4. Browse after an output folder
5. Press Convert button and review in the log UI if everything went good or bad. 
6. Review in the output folder if every files has been generated - depending on the source theme - then review the json file if metadata looks correct.


# Supported themes:
The tool is still in development and may not support all themes perfectly, but it should be able to convert themes from the following launchers:
* YSMenu
* AKMenu
* Moonshell
* TWiLight Menu++
* DSpico Unexported themes (png files with metadata.ini file)

Other themes may be supported as well, but it is not guaranteed that the tool will be able to convert them perfectly, as some themes may have unique features or design elements that may not be compatible with the DSpico Theme format.
At that point, it is better to create an issue in the repository for the new theme support, but it is no guarantee that the new theme support will be implemented, as it depends on the complexity of the theme and the time available for development.


Some themes may need to manually prepare the source theme before conversion, such as adding a metadata.ini file with the correct metadata info, or adjusting the theme structure to be compatible with the DSpico Theme format.


# Images of the program:
![Image of the program in darkmode](/GithubImages/darkmode.png)

![Image of the program in lightmode](/GithubImages/lightmode.png)


# License:
This project is licensed under the MIT License - see the LICENSE file for details.


# Credit:

## Developer of the project:
* Djinnet


## Tools and resources I used for this project:
* Ptexconv tool by Garhoogin
* A fork of the Dark-Mode-Forms. The original project is by BlueMystical and the fork is by Okiedan.
* Documentation from the LNH team of the DSpico theme (https://github.com/LNH-team/pico-launcher/blob/develop/docs/Themes.md) about the file formats and the texture formats.
* Documentation from Discord user (Mow) about how the pico launcher themes works.
* Documentation from reddit user (HogwashDrinker) about how the pico launcher themes works. (https://www.reddit.com/r/flashcarts/comments/1pkf977/guide_how_to_make_themeswallpapers_for_pico/)
* Kernel Themes for providing the themes of YSMenu, AKMenu and Moonshell, so I can test the themes and analyze the file formats and the texture formats. (https://themes.flashcarts.net/)
* TwiLight Menu++ for providing the themes of TWiLight Menu themes, so I can test the themes and analyze the file formats and the texture formats. (https://skins.ds-homebrew.com/)
* ChatGPT + Co-pilot has been partially helpful in generating code snippets and providing suggestions during the development process. But the majority of the code and the logic of the program has been implemented by the developer.