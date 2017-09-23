echo off
rem convert all SCSS files in Scss folder to CSS file save in Styles Folder  
rem not create sourcemap
sass --watch Scss:Styles --sourcemap=none
