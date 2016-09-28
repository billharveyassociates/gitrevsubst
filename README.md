# [Gitrevsubst](http://github.com/desaster/gitrevsubst)

Gitrevsubst is a very simple program to add git revision to a file. The idea is to use it during build time as a replacement for [SubWCRev](https://tortoisesvn.net/docs/release/TortoiseSVN_en/tsvn-subwcrev.html).

## Example of use with Visual Studio

* Add `gitrevsubst.exe` to your PATH
* Make sure `git.exe` is in PATH as well
* In your C# project, rename `Properties\AssemblyInfo.cs` to `AssemblyInfo.cs.tmpl`
* Add something like this to your .tmpl file:
```
[assembly: AssemblyInformationalVersion("1.0.0.$GITDATE$-$GITREV$")]
```
* Add the following command to the pre-build script of your project:
```
"gitrevsubst.exe" "$(SolutionDir).git" "$(ProjectDir)Properties\AssemblyInfo.cs.tmpl" "$(ProjectDir)Properties\AssemblyInfo.cs"
```
* Do not add AssemblyInfo.cs to version control!
* Example result:
```
[assembly: AssemblyInformationalVersion("1.0.0.20162002-44ce0c5")]
```

## Example with dirty status

Use placeholder $GITDIRTY$ to indicate whether the build had uncommitted changes:
```
[assembly: AssemblyInformationalVersion("1.0.0.$GITDATE$-$GITREV$$GITDIRTY$")]
```

### Results
Clean:
```
[assembly: AssemblyInformationalVersion("1.0.0.20162002-44ce0c5")]
```

Dirty (uncommitted changes to files that git is tracking):
```
[assembly: AssemblyInformationalVersion("1.0.0.20162002-44ce0c5-dirty")]
```

## Parsing Tags

Tag the repository in the form "a.b.c" and use the following placeholders: $MAJOR$, $MINOR$, $REVISION$, $BUILD$

If you tag the repository in the form "major.minor.revision", `git describe --always --dirty --tags` will produce an output like `1.2.0-17-gb20a9ea` where 1, 2 and 0 are from the tag ($MAJOR$, $MINOR$, $REVISION$) and 17 is the number of commits since the tag (interpreted as $BUILD$)

eg:

tag = "1.2.3"

```
[assembly: FileVersion("$MAJOR$.$MINOR$.$REVISION$.$BUILD$")]
[assembly: AssemblyInformationalVersion("$MAJOR$.$MINOR$.$REVISION$.$BUILD$-$GITREV$")]
```

### Results
```
[assembly: FileVersion("1.2.3.17")]
[assembly: AssemblyInformationalVersion("1.2.3.17-44ce0c5")]
```
