# Remove actions after a certain date (currently December SGX)
cat gourceLog.txt | awk -F\| '$1<=1639785600' > gourceLog.temp
sed -i.bak '/Packages/d' ./gourceLog.temp
sed -i.bak '/ProjectSettings/d' ./gourceLog.temp
sed -i.bak '/Plugins/d' ./gourceLog.temp
sed -i.bak '/Polybrush/d' ./gourceLog.temp
sed -i.bak '/TextMesh/d' ./gourceLog.temp
sed -i.bak '/\.meta/d' ./gourceLog.temp
mv gourceLog.temp gourceLog.txt
rm gourceLog.temp.bak

# Setup Project Name
projName="Grave Robber at Large - Unity 3d Project"

function fix {
  sed -i -- "s/$1/$2/g" gourceLog.txt
}

# Replace non human readable names with proper ones
fix "|berriers|" "|Prof. B|"
fix "|seaverj|" "|Prof. Seaver|"
fix "|vangj2009|" "|Joseph Vang|"
fix "|twedena2987|" "|Aaron Tweden|"
fix "|shermanr3901|" "|Renell Sherman|"
fix "|linkd5625|" "|Damian Link|"
fix "|boehmz8613|" "|Zachary Boehm|"
fix "|andersond4505|" "|Declin Anderson|"
fix "|woodm1876|" "|Maria Wood|"
fix "|woodc1611|" "|Carter Wood|"
fix "|turrellj9540|" "|James Turrell|"
fix "|norgrenl4661|" "|Nord Norgren|"
