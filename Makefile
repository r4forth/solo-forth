# Makefile

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Last modified: 201604161830

# ==============================================================
# Author

# Marcos Cruz (programandala.net), 2015, 2016.

# ==============================================================
# License

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and this
# license.  There is no warranty.

# ==============================================================
# Requirements

# head, cat and sort (from the GNU coreutils)

# zmakebas (by Russell Marks)
#   Usually included in Linux distros. Also see:
# 	http://sourceforge.net/p/emuscriptoria/code/HEAD/tree/desprot/ZMakeBas.c
# 	https://github.com/catseye/zmakebas
# 	http://zmakebas.sourcearchive.com/documentation/1.2-1/zmakebas_8c-source.html

# bin2code (by Metalbrain)
# 	http://metalbrain.speccy.org/link-eng.htm

# fsb2 (by Marcos Cruz)
# 	http://programandala.net/en.program.fsb2.html

# mkmgt (by Marcos Cruz)
# 	http://programandala.net/en.program.mkmgt.html

# Forth Foundation Library (by Dick van Oudheusden)
# 	http://irdvo.github.io/ffl/

# ==============================================================
# History

# See at the end of the file.

# ==============================================================
# To-do

# XXX FIXME -- the loader and the disk image are built even when the
# sources are update, 

# ==============================================================
# Notes

# $@ the name of the target of the rule
# $< the name of the first prerequisite
# $? the names of all the prerequisites that are newer than the target
# $^ the names of all the prerequisites

# ==============================================================
# Config

VPATH = ./

MAKEFLAGS = --no-print-directory

#.ONESHELL:

# ==============================================================
# Main

.PHONY: all
all: gplusdos
#all: gplusdos plus3dos

.PHONY: gplusdos
gplusdos: solo_forth_disk_1.mgt solo_forth_disk_2.mgt

.PHONY: plus3dos
plus3dos: solo_forth_disk_a.dsk
# plus3dos: solo_forth_disk_a.dsk solo_forth_disk_b.dsk

clean: cleantmp
	-rm -f solo_forth_disk_?.mgt solo_forth_disk_?.dsk

cleantmp:
	-rm -f tmp/*

include Makefile.pasmo

# XXX OLD
# .PHONY : pasmo
# pasmo:
# 	@make Makefile.pasmo
# .PHONY : as
# as:
# 	@make Makefile.binutils

# ==============================================================
# BASIC loader

# The BASIC loader of the system is coded in plain text. The addresses
# that depend on the kernel (its load adresses and entry points) are
# represented in the source by labels. A Forth program replaces the
# labels with the actual values, extracted from the symbols file
# created by the assembler. Then zmakebas converts the patched loader
# into a TAP file, ready to be copied to a disk image.

# The

# ----------------------------------------------
# For G+DOS

tmp/loader.gplusdos.bas: \
	tmp/kernel.symbols.gplusdos.z80s \
	src/loader/gplusdos.bas
	gforth tools/patch_the_loader.fs $^ $@

tmp/loader.gplusdos.bas.tap: tmp/loader.gplusdos.bas
	zmakebas -n Autoload -a 1 -o $@ $<

# ----------------------------------------------
# For +3DOS

tmp/loader.plus3dos.bas: \
	tmp/kernel.symbols.plus3dos.z80s \
	src/loader/plus3dos.bas
	gforth tools/patch_the_loader.fs $^ $@

tmp/loader.plus3dos.bas.tap: tmp/loader.plus3dos.bas
	zmakebas -n DISK -a 1 -o $@ $<

# ==============================================================
# Font drivers

# These drivers are not part of the Solo Forth library yet.
# Meanwhile, their binary files are included in the first disk.

# Note: an intermediate file called "4x8fd.bin" is used, in
# order to force that filename in the TAP header and therefore
# in the disk image.  This file must be in the current
# directory, because the path specified in the command will be
# part of the filename in the TAP header.

# XXX WARNING -- 2016-03-19. bin2code returns error 97 when one
# the filenames has a path, but it creates the tap file as
# usual.  A hyphen at the beginning of the recipe line forces
# make to ignore the error.

tmp/4x8fd.tap: src/modules/4x8fd.z80s
	pasmo --tap $< 4x8fd.bin ; \
	mv 4x8fd.bin $@

tmp/prnt42.tap: bin/modules/prnt42.bin
	cd bin/modules/ ; \
	bin2code prnt42.bin prnt42.tap 63610 ; \
	cd - ; \
	mv bin/modules/prnt42.tap tmp/prnt42.tap

# ==============================================================
# Fonts

# The DSK disk image needs the fzx files to be packed into TAP
# first.

fzx_fonts = $(wildcard bin/fonts/*.fzx)

bin/fonts/fzx_fonts.tap : $(fzx_fonts)
	cd bin/fonts ; \
	for file in $$(ls -1 *.fzx); do \
		bin2code $$file $$file.tap; \
	done; \
	cat *.fzx.tap > fzx_fonts.tap ; \
	rm -f *.fzx.tap ; \
	cd -

# ==============================================================
# The disk images

# ----------------------------------------------
# Main disk

# The first disk ("1" for G+DOS, "A" for +3DOS) contains the
# Forth system. The user will use the first disk for customized
# versions of the Forth system, Forth turnkey applications,
# fonts, graphics and data.

solo_forth_disk_1.mgt: \
		tmp/loader.gplusdos.bas.tap \
		tmp/kernel.gplusdos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap
	mkmgt solo_forth_disk_1.mgt \
		bin/sys/gplusdos-sys-2a.tap \
		tmp/loader.gplusdos.bas.tap \
		tmp/kernel.gplusdos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap \
		bin/fonts/*.fzx

tmp/solo_forth_disk_a.tap: \
		tmp/loader.plus3dos.bas.tap \
		tmp/kernel.plus3dos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap \
		bin/fonts/fzx_fonts.tap
	cat \
		tmp/loader.plus3dos.bas.tap \
		tmp/kernel.plus3dos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap \
		bin/fonts/fzx_fonts.tap \
		> tmp/solo_forth_disk_a.tap

solo_forth_disk_a.dsk: tmp/solo_forth_disk_a.tap
	tap2dsk -720 -label SoloForth \
		tmp/solo_forth_disk_a.tap \
		solo_forth_disk_a.dsk

# ----------------------------------------------
# Library disk

# The second disk ("2" for G+DOS, "B" for +3DOS) contains the
# source blocks of the library. The blocks are stored on the
# disk sectors, without file system.

library_files = $(sort $(wildcard src/lib/*.fsb))

# Library disk for G+DOS

# XXX WARNING -- `%` in filter patterns works only at the start
# of the pattern?

gplusdos_library_files = \
	$(filter-out %plus3dos.fsb %idedos.fsb , $(library_files))

tmp/library_for_gplusdos.fsb: $(gplusdos_library_files)
	cat \
		$(gplusdos_library_files) \
		> tmp/library_for_gplusdos.fsb

solo_forth_disk_2.mgt: tmp/library_for_gplusdos.fsb
	fsb2-mgt tmp/library_for_gplusdos.fsb ;\
	mv tmp/library_for_gplusdos.mgt solo_forth_disk_2.mgt

# Library disk for +3DOS

plus3dos_library_files = $(filter-out %gplusdos.fsb,$(library_files))

tmp/library_for_plus3dos.fsb: $(plus3dos_library_files)
	cat \
		$(gplusdos_library_files) \
		> tmp/library_for_plus3dos.fsb

# XXX TODO -- `fsb2-dsk` is not ready yet.
solo_forth_disk_b.dsk: tmp/library_for_plus3dos.fsb
	fsb2-dsk tmp/library_for_plus3dos.fsb ;\
	mv tmp/library_for_plus3dos.dsk solo_forth_disk_b.dsk

# ==============================================================
# Backup

.PHONY: backupsrc
backupsrc:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth_src.tar.xz \
		Makefile* \
		src/lib/*.fsb \
		src/kernel.z80s

.PHONY: backuplib
backuplib:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth_library.tar.xz \
		src/lib/*.fsb

# XXX TODO --
# .PHONY: backupkernel
# backupkernel:
# 	xz src/kernel.z80s
# 	mv src/kernel.z80s.xz backups/$$(date +%Y%m%d%H%M)_solo_forth_kernel.z80s.xz \

# XXX OLD
.PHONY: oldbackup
oldbackup:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth.tar.xz \
		_draft/* \
		_old/* \
		_tests/* \
		src/* \
		Makefile* \
		*.adoc \
		*.mgt \
		*.dsk \
		*.sh \
		*.txt

# ==============================================================
# History

# 2015-06-02: Start.
#
# 2015-06-17: Improved backup recipe.
#
# 2015-06-29: Improvement: The source filenames are configurable. This
# makes it easier to try old versions, for debugging.
#
# 2015-07-22: Added the MGT disk images to the backup.  Sometimes it's
# useful to test and old version without recompiling the old sources.
#
# 2015-08-14: Updated backup recipe.
#
# 2015-08-17: Modified to use GNU binutils instead of Pasmo.
#
# 2015-08-18: Improved. New: a Forth program creates the symbols file.
#
# 2015-08-20: Divided in three parts: Makefile, Makefile.pasmo,
# Makefile.binutils.
#
# 2015-10-10: Substituted fsb (written in Vim) with fsb2 (written in
# Forth for Gforth). fsb was becoming too show with more than 300
# source screens, while fsb2 is instantaneous.
#
# 2015-10-15: Updated.
#
# 2015-11-10: First changes to support also the +3DOS version.
#
# 2015-11-11: The DOS error codes are separated from the main file of
# the library.
#
# 2015-11-12: Fixed the load address of the font drivers; they were
# missing because of the recent use of `bin2code`, required to build
# +3DOS disk images.
#
# 2016-02-15: Added Nuclear Invaders to the library, a game under
# development for Solo Forth, in order to try it.
#
# 2016-02-22: Delete also <library.complete.*.fsb> in clean.  It seems
# this is required when parts of the library are simbolyc links.
#
# 2016-03-19: Updated after the reorganization of files into
# directories.
#
# 2016-03-22: Removed Nuclear Invaders from the library, because now
# it's built on its own directory.
#
# 2016-03-24: New partial backups.
#
# 2016-04-13: Improved: the BASIC loader is patched with the current
# memory addresses, extracted from the Z80 symbols file. Updated the
# requirements and the license.
#
# 2016-04-16: Fix: the patching of the loader didn't work after `make
# clean`, because a prerequisite was missing.
#
# 2016-04-16: Also the +3DOS loader is patched with the current values
# of the kernel.
