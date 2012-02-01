pushd ..\src\JeffWilcox.Maps\
msbuild /p:Configuration=Release
popd
copy ..\src\JeffWilcox.Maps\bin\release\* .\JeffWilcox.Maps\lib\sl4-windowsphone71\
nuget pack JeffWilcox.Maps\JeffWilcox.Maps.nuspec -o .\
echo Don't forget to publish if need be!
