echo "Publishing Project..";
dotnet publish -c Release Splitter/Splitter.ConsoleApp/;

echo "Making Splitter Directory..";
mkdir /usr/local/bin/splitter;

echo "Copying Artifacts..";
cp -r -v Splitter/Splitter.ConsoleApp/bin/Release/netcoreapp2.0/publish/ /usr/local/bin/splitter;