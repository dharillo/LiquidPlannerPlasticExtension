# LiquidPlanner - Plastic SCM integration
## Installation
### Step 1:
Right now, the project is configured to deploy directly on the usual installation
folder of the *Plastic SCM* folder. There is no current installer per se. You
will need to build the project using *Visual Studio 15* in administrator mode.

In the future a better way of installing the extension will be developed.

You can change the path of the release in the configuration of the project clicking
*Project* -> *LiquidPlannerPlasticExtension* -> *Build* -> *Output path*

### Step 2:
The second step to be done before using the extension is to add our extension to
the *customextensions.conf* file. This file can be found in the following path:
**PLASTIC_INSTALLATION_FOLDER/client/customextensions.conf**
where *PLASTIC_INSTALLATION_FOLDER* is the location of PlastiSCM5. Usually found
in: C:\ProgramFiles\PlastiSCM5

In that file we need to add the following line at the end of the file:
`LiquidPlanner=LiquidPlannerPlasticExtension.dll`

You will need to open the editor used in administrator mode to be able to change
the content of that file.

## Plugin configuration
Once installed, we will be able to configure the plugin using the *Preferences*
window of the *Plastic SCM* client.
There, in the Issue Tracker entry, we will be showed the option of using *LiquidPlanner*
as issue tracker.

The parameters that you will need to configure are:
* User: the email used to log in LiquidPlanner
* Password: your password for LiquidPlanner. Due to some problems with the Password
field type in the extension offered library, the password is not hidden.
* Workspace: the numeric identifier of the workspace in LiquidPlanner. You can know
this ID easily looking at the URL when you access to LiquidPlanner in a browser.
The URL will be somthing similar to: `https://app.liquidplanner.com/space/<workspaceId>/projects`
where workspaceId is the number you must insert.
* Branch prefix: Prefix to add to all the branches created from now on when you
do it using a task in liquidplanner as information source.

Once configured, you can check if all works using the *Test connection* button.

## Goals
This project wants to offer the possibility of using [LiquidPlanner](https://app.liquidplanner.com/)
as Issue Tracker tool withing the Plastic SCM client.

## References
The code of this project was developed using the [api-examples](https://github.com/LiquidPlanner/api-examples)
offered by *LiquidPlanner*. In that repository you can find examples using the 
API offered by *LiquidPlanner* in different languages.

An explanation of the API can also be found in this [PDF](https://www.liquidplanner.com/assets/api/liquidplanner_API.pdf).

In the other hand, the guide to develop extensions for the *Plastic SCM* client
can be found [here](https://www.plasticscm.com/documentation/extensions/plastic-scm-version-control-task-and-issue-tracking-guide.shtml#WritingPlasticSCMcustomextensions).

As you may be able to see, a lot of the code developed in this project is almost
identical to the examples given in those to sources of information.