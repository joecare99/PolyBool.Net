# PolyBool.Net
Boolean operations on polygons (union, intersection, difference, xor)<br/>
(this library is a port for .net of [polybooljs](https://github.com/velipso/polybooljs))
## Features

1. Clips polygons for all boolean operations
2. Removes unnecessary vertices
3. Handles segments that are coincident (overlap perfectly, share vertices, one inside the other,
   etc)
4. Uses formulas that take floating point irregularities into account (via configurable epsilon)
5. Provides an API for constructing efficient sequences of operations

## Resources

* [View the demo + animation](https://rawgit.com/voidqk/polybooljs/master/dist/demo.html)
* Based somewhat on the F. Martinez (2008) algorithm:
    * [Paper](http://www.cs.ucr.edu/~vbz/cs230papers/martinez_boolean.pdf)(offline)
    * [Paper-Achive](https://github.com/akavel/martinez-src/blob/master/paper.pdf)
    * [Code](https://github.com/akavel/martinez-src)
 
## Tests:
[![.NET Core Desktop](https://github.com/joecare99/PolyBool.Net/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/joecare99/PolyBool.Net/actions/workflows/dotnet-desktop.yml)

