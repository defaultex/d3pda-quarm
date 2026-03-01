This is an adaptation of my 'D3PDA' interface for EverQuest compatible with Project Quarm w/ Zeal.

# D3PDA
This started as a UI themed after the PDA in Doom 3 and evolved from there into something unique. This UI utilizes a few tricks that you don't see in other UIs to achieve it's unique file structure and layout features. This UI grew to such complexity that I wrote a generator program to handle generating the variations of files that only differ in what data they convey.

### What's included in the UI
* A re-skin of the default templates.
* Player window with an integrate stat sub-window, server tick indicator, and xp per hour counters.
* Group window.
* Spell Casting window with tinted spell slots, spell names, integrated casting gauge, and recast gauges.
* Target window.
* Buff window with variations for left and right aligned (see EQUI_BuffWindow.xml for details).
* Song window with variations for left and right aligned (see EQUI_ShortDurationBuffWindow.xml for details).
* Loot window with smaller items and more columns.
* Merchant window similar to the loot window.
* Spell gem and buff icons based on 90s cRPGs.
* Spell book with the window background removed.

### Screenshot
![d3pda-preview](https://github.com/user-attachments/assets/adcff3db-f7f3-4b4b-8f5e-822cf36eeb26)

### Generator Notes
* The generator looks through the folders in the UI folder for files ending with '.uigencfg'.
* A '.uigencfg' file configures the generator to produce a sequence of files based on a template.
* * Global parameters that are shared amongst all generated files.
* * Local parameters that provide a value for each generated file.
* * Text replacement utilizing regex on specified files.
* * Element copy operations from the generated files to a destination file.
* The parser is capable of referencing EQ types by name, for example 'EQLabelType.Name'.
* The parser is also capable of performing a .Net math expression with access to system and config parameters.