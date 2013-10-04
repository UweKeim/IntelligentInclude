# Zeta Intelligent Include

This project presents you a Windows small command line tool to mimic the Persistent Includes syntax of the BBEdit editor to include files into other files while preserving the include directive itself.

## Overview

The tool mimics the `#bbinclude` syntax for the Persistent Includes feature of the BBEdit editor tool to inline-include files into other files while preserving the include directive itself after the include.

It supports any file that is acceptable to contain include directives, usually inside comments in the language of the document type.

You would use this tool usually inside a windows Command script ("Batch script") to process multiple files. The idea of keeping the include directives in-place is helpful when you want to include the external files mutliple times over and over again. You can use it to produces files that normally don't support including other files natively (e.g. include a HTML fragment inside another HTML file without having to use server-side include technologies; great for e.g. Dropbox-published files).

Our web designer use this tool to manage shared code (HTML, CSS and JavaScript) when editing the templates in our [Desktop Content Management System](http://www.zeta-producer.com) Zeta Producer CMS.

## Usage

    ii.exe <path to folder or file or wildcard> [/r]

First parameter is any path to a folder or a folder with wildcards.
Second, optional parameter to specify whether to recurse folders.

## Examples

    ii \"C:\\MyFolder\\*.css\" /r

Process all CSS files in C:\\MyFolder and all subfolders.

    ii \"C:\\MyFolder\\MyFile.html\"

Processes the single file MyFile.html.

## Description

The program parses input text files for include directives
and inline-replaces the lines between the starting include
directive and the ending include directive with the file that
is specified to be included.

An include start directive has the syntax:

    #zetainclude \"..\\myfolder\\filetoinclude.txt\"

An include end directive has the syntax:

    #endzetainclude

The program assumes that both directives are writen in separate
lines in the input file. Therefore you cannot write both in the
same line.

Usually you would write the include directives inside comments,
so that the include would not break the meaning of the file itself.

Since the program makes no assumption regarding comments that
surround the include directives, you can use it within any file
that supports comments like e.g. CSS, HTML.

Real-world example (inside an HTML file):

    <!-- #zetainclude \"..\\myfolder\\filetoinclude.html\" -->
    <!-- #endzetainclude -->

Other real-world example (inside a CSS file):

    /* #zetainclude \"..\\myfolder\\filetoinclude.css\" */
    /* #endzetainclude */

## Remarks

  * Currently the program works best with input and include files both are being UTF-8 encoded.
  * Recursive includes are supported. I.e. an included file may include other files, too.
  * You may have multiple include directives inside a single file.
  * Currently we do only support a limited subset of the syntax the the BBEdit editor supports. E.g. we do not support including files with variables. See the [BBEdit manual](pine.barebones.com/manual/BBEdit_10_User_Manual.pdf), appendix C for a full documentation of the BBEdit program.

## History

  * *2013-10-04* - First public release to Github.
