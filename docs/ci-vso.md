# Continuous Integration with Visual Studio Online 

### Prerequisites

You'll need the following to be able to complete this code lab:

* A [Visual Studio Online account](https://www.visualstudio.com/)

## Create New VSO Project

![New team project](../images/vso-new-team-project.png)

From your Visual Studio Online account homepage ([VSO Account Homepage](https://app.vsaex.visualstudio.com)) click the 'New team project' link and provide a name and description (doesn't matter what).

![New team project dialog](../images/vso-new-team-project-dialog.png)

Make sure that you select 'Git' under 'Version control'

## Pull example solution from Github

Once your new repo is created you can clone the contents directly from Github.

![Setup repo options](../images/vso-setup-repo.png)

Expand the 'or import a repository' section and click 'Import'

![Import from Git repo](../images/vso-import-git-repo.png)

When prompted enter a clone URL of `https://github.com/Gilmond/ci-vso.git` - hit 'Import' and VSO will pull the source from Github.

You can then check the contents of your repo through the web UI. You should have the following:

![New repository file](../images/vso-new-repository.png)

### Example solution structure

The example solution is very basic and covers a few simple scenarios for our CI process. It has:

* a common library (src\ci-vso-lib)
* a console app (src\ci-vso)
* a web app (src\ci-vso-web)
* a test library (tests\ci-vso-tests)

What does it do? Not much - but just enough to prove the point - a FizzBuzz implementation.

## Setting up a CI build definition

We're going to build up our build definition piece by piece - running the build at each step to verify what it's doing / how.

The aim of this definition is to compile all the example code and run all the tests within it. We're trying to achieve this as quickly as possible to reduce our mental round-trip times (the gap between believing you're finished with a task, and hence your brain "unshelving" the details and finding out there's a problem and having to bring all that detail back to the fore - the shorter the _better_).

From the top nav bar click on 'Build & Release'

![Build and release](../images/vso-build-and-release.png)

You should get a screen telling you there are no definitions. So click 'New definition' and get started:

* Use an empty template - this way you'll know exactly what steps you've added
* Notice a step has already been added called 'Get sources' - this will pull the source from the Git repository
* Set the name of the build to Master-CI
* Select the 'Options' tab and on the right hand side open the drop down 'Default agent queue' and select 'Hosted VS2017'
* Select the 'Tasks' tab
* Add a '.NET Core' step
    * Change the command to `restore`
    * Enter `**\*.csproj` in the Projects field
* Hit 'Save & queue' - you'll be presented a popup where you can customise build variables and the branch. Leave everything as it is:

![Queue build](../images/vso-queue-build.png)

Your build will be queued up:

![Queued build](../images/vso-queued-build.png)

Hopefully - in about 30 seconds (depending on queueing time) - you'll get a successful build. Click through to the instance of the build and you'll be able to inspect the steps that were executed and their outputs.

All we've done so far is restore our NuGet packages - we still need to:

* Build
* Run tests
* Publish test results
* Package up the build output

### Building the source

Edit your build definition and do the following:

* Select the 'Variables' tab
* Click '+ Add'
    * For name, enter : `BuildConfiguration`
    * For value, enter : `release`
    * Make sure the 'Settable at queue time' checkbox is ticked - this allows us to change the build configuration when we queue a build - so we can produce debug builds
* Return to the 'Tasks' tab
* Click 'Add Task'
* Select the .NET Core task and click 'Add'
* You'll note the new task has been defaulted to the command 'build'
* Set the projects field to `**\*.csproj`
* Set the arguments field to `--configuration $(BuildConfiguration)`
    * The `$(BuildConfiguration)` part will be expanded by the build process with the value in the variable we just defined.
* Hit 'Save & queue', notice in the popup there's now an entry for the variable we defined:

![Build variables](../images/vso-build-variables.png)

### Running tests and publishing results

Your build _should_ still be successful. Not for long though... now we're going to enable running of our tests and publishing the results into VSO, so re-edit your definition:

*Run unit tests*

* Select the 'Tasks' tab
* Click 'Add Task'
* Select the .NET Core task and click 'Add'
* Change the command to `test`
* Set the projects field to `tests\**\*.csproj`
* Set the arguments field to `--configuration $(BuildConfiguration) --logger:trx`

*Publish results*

* Click 'Add Task'
* Select the Publish Test Results task and click 'Add'
* In the Test result format dropdown select `VSTest`
* Set the Test results files field to `**\*.trx`
* Expand the Control Options section 
* In the Run this task dropdown, select `Even if a previous task has failed, unless the build was cancelled`
* Hit 'Save & queue'

![Broken build](../images/vso-broken-test.png)

Bad times - your build is now broken - thanks to the _cough_ deliberate bug. Look at the details of the broken build and click the 'Tests' subheading:

![Failed test](../images/vso-failed-test.png)

From here you _should_ be able to deduce the problem - but hold your horses, don't fix it yet - we're going to add a trigger to the build first.

### Setting up integration triggers

Edit your build definition again:

* Select the 'Triggers' tab 
* On the left hand side flick the Continuous Integration switch to 'Enabled'
* Leave all the other values as their defaults
* Hit 'Save'

### Fix test break

Now when you push a change to fix your broken test you'll immediately get a build queued for that change.

So long as you've found the _cough_ deliberate mistake you should see a verdant field of integration glory:

![Working tests](../images/vso-working-tests.png)

### Enable publishing of build output

Edit your build definition again:

*Publish website*

* Select the 'Tasks' tab
* Click 'Add Task'
* Select the .NET Core task and click 'Add'
* Change the command to `publish`
* Set the arguments field to `--configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)`

*Publish CLI tool*

* Click 'Add Task' again
* Select the .NET Core task and click 'Add'
* Change the command to `publish`
* Set the arguments field to `--configuration $(BuildConfiguration) --output $(build.artifactstagingdirectory)`
* Uncheck the 'Publish Web Projects' checkbox
* Set the Projects field to `src\ci-vso\*.csproj`

*Put published components into artifacts of the build*

* Click 'Add Task' again
* Select the Publish Artifact task and click 'Add'
* Set the Path to Publish field as `$(build.artifactstagingdirectory)`
* Set the Artifact Name to `drop`
* Set the Artifact Type field to `Server`
* Hit 'Save & Queue'

Once this build completes you'll have a new top level tab in your build report 'Artifacts'. You can either download a `drop.zip` file that will contain your build output or view it's contents in the webapp - either way you should see it contains two zip files : `ci-vso-web.zip` and `ci-vso.zip` which are the output of our two 'end-point' projects.

Your final build definition should look like:

![Final build process](../images/vso-final-build-process.png)

You can rename each step to be more descriptive:

* dotnet restore to `Restore NuGet packages`
* dotnet build to `Build Projects`
* dotnet test to `Run Tests`
* Publish Test Results **\\*.trx to `Publish Test Results`
* 1st dotnet publish to `Create Website Artifacts`
* 2nd dotnet publish to `Create CLI Artifacts`
* Publish Artifact: drop to `Publish Artifacts`

![Renamed build process](../images/vso-renamed-build-process.png)

### Code Coverage

Alas this isn't currently supported for .NET Core - it's coming, just not yet. 

# Bonus Steps

If you've got to the end of this before we run out of time, or you just want to take things a little further - here's some additional things you can try adding to your build.

* Define a build number schema - at the moment your builds are numbered sequentially, so the first one is `1`, second `2` and so on. It's possible to define build numbers based on a variety of factors including todays date.
* Setup a CD (Continuous Deployment) pipeline. Create an Azure website and add steps to the build so that your website is automatically published on _every_ build.
    * You might want to clone your build definition to create a manually triggered `Master-Deployment` build. You don't _always_ want every checkin to be deployed.
    * There's an `Azure App Service Deploy` task that will help with that.
* Get your source from the React lab into a VSO repository and use an `npm` task to build your app

# _**Don't read this if you don't want to know what the test break was**_

One of the test cases in FizzBuzzerTests.cs is wrong - it states the expected result is `Bizz` when it should be `Buzz` - it's on line 25 - `[InlineData(5, "Bizz")]`