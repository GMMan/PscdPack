SEGA Genesis Classics ROM Packer
================================

This program allows you to extract ROMs from SEGA Genesis Classics .pak files and
to make your own for use with the emulator.

The user interface is rather self-explanatory. Here are a few notes about thing that
are not:

- The New/Open button creates a new pack or opens an existing pack. If the pack you
  are trying to open was made from replacing ROM with the program but you didn't click
  on Save, the file will fail to open. You should delete the file and create a new one.
- For PAL ROMs, select Europe from regions.
- Thumbnails should be 160x60 BMPs. However, you can put anything you want for the
  thumbnail, including non-image files. The packer will assume a properly sized BMP,
  but if it isn't, it'll just display a blank space where the thumbnail is supposed to be.
- Most packs don't have thumbnails, and they aren't used by the emulator.
- When you replace ROM, the packer will try to automatically fill in the fields. Adjust
  the values if anything's incorrect (for example, you may want to use PAL for worldwide
  ROMs, in which you would select Europe for regions).
- The pack file is locked while it's open. If you want to try the pack without exiting
  the program, click close and the file will be unlocked so it can be read by the emulator.

Important: the ROM image must be a raw dump. If you have a SMD/MD file, you will have to
convert it to BIN first. For reference: http://www.emulatronia.com/doctec/consolas/megadrive/genesis_rom.txt

On-cartridge Memory Config
--------------------------
For ROMs that came from cartridges with built-in SRAM or EEPROM, you will need to set
this section. The dropdown contains the memory mode. The first textbox is the page mask,
which is the address that the SRAM is mapped to, right shifted 12 bits. The second textbox
is the SRAM size. It is the number of 256-byte pages the SRAM provides. Both textboxes are
in 16-bit hex. See https://genplus-gx.googlecode.com/files/gen_eeprom.pdf for a list of
games that contain EEPROM and for their configuration.

Note: EEPROM saving may not work for non-SEGA games because of different mappers/modes used.

DisableBankSwitching mode disables the bank switching mechanism used in Super Street Figher
II. See http://emu-docs.org/Genesis/ssf2.txt for more information. You typically do not
need to use this mode.

Emulator Limitations
--------------------
The following limitations applies on the emulator, so not every pack made will be accepted:

- Compressed pack data must be smaller or equal to 4MiB
- Uncompressed ROM must be smaller or equal to 4MiB
- If you are using the PscdFormat class in the program, you can choose to not flip words.
  However, the emulator will refuse to load non-flipped ROMs.
- Packs must have no trailing junk bytes.
- ROMs operating EEPROM in mode 3 may not work.
- EA (with EEPROM) and J-Cart ROMs may not work.
- Any ROM using more than 256 bytes of EEPROM may not work.
