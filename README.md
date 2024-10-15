<img src="DATa Archiver/DATa Archiver256.png" width="256">

# DATa Archiver

Open, Unpack, and Create .dat files that use the 'archive  v2.DMZ' format.

Localized for 13 languages, not including English! I am very proud of my method for switching text between languages.

# What is this .DMZ thing?

These types of files can be found in the game Sonic Adventure DX. It is a way of packing multiple files into one, but does not offer encryption or compression.

Creating this program has been a fun dive into very light reverse engineering as I could find no information about this file format online.

I believe that either SEGA or CRI Middleware created this file format and do not know if some SEGA games use it or if CRIware games from a specific era may use it.

I've written documentation on how this format works and it is included with the source code.

# How To Use

You can either open a .dat file to unpack its contents or start fresh by adding files to the program to create a new .dat file.

In my testing, using this program to create a .dat file with the same files that SEGA used results in a byte-for-byte match.

If you open an incompatible file the program should cancel and let you know.

Please note, you cannot add files directly to an existing .dat file. You must first unpack the file, add all the files you want, and then save. Unfortunately, I didn’t have enough time to make this process more seamless. Similarly, you cannot add a .dat file to files you’ve already added without first unpacking.

# Limitations

To ensure 100% compatibility with SEGA’s files, there is a 4GB limit per file.

It is possible a game or program may have a 4GB or even 2GB limit as a whole for the entire archive so please keep that in mind too.

The maximum number of files you can add depends on the length of the filenames, expect a limit of around 100 files.

The program has built-in checks to prevent exceeding these limits. If you enable debug mode, the tool will show how much space is left after each file import.

Enable debug mode by launching the program from the command line with the -d option.

<img src="DATa Archiver/git_ad01.png" width="798">
<img src="DATa Archiver/git_ad02.png" width="802">
