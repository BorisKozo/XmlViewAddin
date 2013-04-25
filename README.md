XmlViewAddin
============
# General
I am going to create a new VuGen addin which allows you to view	XML content in
a separate window directly from the script. My goal is not to provide some highly
useful functionality but to introduce you to the VuGen extensibility mechanism.

## Addins
### Directory Structure
VuGen 11.5x is based on [SharpDevelop 4.1](http://www.icsharpcode.net/opensource/sd/) and therefore it 
uses the SharpDevelop (SD) extensibility layer. We added an additional layer of services but more on that later.
VuGen itself contains only the core extensibility layer and the application starter, all the other functionality
including code editing, recording, replaying, script management, etc... are extensions to VuGen written in the same
way I am going to present here. All the extensions are defined in the *%LR_DIR%\addins* directory through the use of
```.addin``` files. Under the aforementioned directory you can find the following core addins:

* ```ICSharpCode.SharpDevelop.addin``` - Defines the basic UI structures we take from SharpDevelop (e.g. main menu, main toolbar, etc...).

* ```VuGenGeneralUI.addin``` - Defines VuGen specific UI structures for generic use (e.g. search).

* ```VuGenBackEnd.addin``` - Defines functionality needed for VuGen business logic.

The rest of the addins are distributed throughout the subdirectories by topic.
As you can see, VuGen is separated into three layers - SharpDevelop, Utt, and VuGen. The reason
for this separation is not important to this discussion.

### Some useful terms
By now you are familiar with the term ```addin``` but where do those addins go? The addins
are combined during the application startup into one big tree-like structure known as the 
```addin-tree```. The addin tree is built of nodes where each node contains ```codons```. A ```codon```
is basically a piece of code that "knows" how to do something. For example, if I want to add a new menu item
I create a ```codon``` which implements an interface of a menu item (I show an example of this later) and add a
definition of that ```codon``` to the appropriate place in the addin file. To tell the addin tree to which node we want
to add our ```codon```, we use the ```path``` property which is a concatenated list of all the nodes in the route to the target node.
The path uses the ```id``` property of any ```codon``` to generate a new node for this ```codon``` so essentially each ```codon``` generates
a node in the tree. For example, if I want to add my menu item into the main menu of the application, I should put it into the ```/SharpDevelop/Workbench/MainMenu``` path.
This path basically reads, root -> SharpDevelop -> Workbench -> MainMenu node.


### Addin file structure
The addin file is a simple XML file with a predefined structure. I briefly describe the most
important (and mandatory) parts of this file but you can always learn more from the SD website.
The following is an example of a simple addin file:
```xml
<AddIn name = "XmlViewAddin"
       author = "Boris Kozorovitzky"
       description = "Adds the functionality to easily view XML data directly from the editor">
  
  <Runtime>
    <Import assembly = "XmlViewAddin.dll"/>
  </Runtime>

  <Manifest>
    <Identity name="XmlViewAddin" version = "0.0.1.0" />
  </Manifest>

  <Path name = "/SharpDevelop/Workbench/MainMenu">
    <MenuItem id = "MenuItemTest" label = "TEST!" type="Menu"></MenuItem>
  </Path>

</AddIn>
```

The main element ```<AddIn>``` contains basic information on the the addin file and the author.
Inside we find the ```<Runtime>``` element. This element is very important because it links the 
```.addin``` file with all the dlls implementing its functionality. In this example I import only
the dll which I am going to produce for this addin example called ```XmlViewAddin.dll```. You can specify
as many dlls as you need in this section, each in its own ```<Import>``` element.

The ```<Manifest>``` element contains the metadata about your addin. In the example I specify only the
identity of my addin and the version number. This is a mandatory element because it is needed to register
your addin with the extensibility layer.

The ```<Path>``` element tells the extensibility layer where to put the ```codons``` within the Addin-Tree.
In the example we can see that we add a menu item with the label "TEST!" to the main menu of our application.

## Writing your own addin
In this section I describe the process of writing my addin. I am using Visual Studio 2010 but
you can do it in any editor. Specifically, we ship VuGen with SharpDevelop 4.1 (including sources) so
you can just install that from the LR DVD here ```\Additional Components\Third Parties\SharpDevelop_4_1_src\SharpDevelop-4.1.0.8000.zip```.

I start by creating a C# class library and adding a ```.addin``` file to it. You can use any .Net language
as long as you compile to framework 4 (or above). My addin file contains the XML from the example above.
The next thing I do is to configure the class library to be built into the following folder: *%LR_DIR%\addins\extra\XmlViewAddin*
(create this folder manually first) and the build action of my ```.addin``` file to _Copy always_. This will copy the 
update version of my addin directly to VuGen every time I build it. 
For easier debugging I set the Start Action of my debugger to start VuGen by pointing it to *%LR_DIR%\bin\vugen.exe*.
Run the addin (press F5), a VuGen should start and the main memu should contain our item:

![VuGen with the new menu item](/img/i1.png "VuGen with the new menu item")

