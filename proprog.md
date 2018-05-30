# Professional Programming - Kristoffer Baardseth
Repository for IMT3602 Professional Programming - code examples and readme

## Thesis
### Bachelor Thesis Team members
Bjørn K. Aune

Kristoffer Baardseth

Benjamin G. Wendling

### Repositories
Thesis project - https://bitbucket.org/bgwendling/bachelor_project_eyevr/src/master/

## Group Discussion
### Language(s) we used
I mainly used C# when programming for the project, but GO was also used for the server. I will not discuss GO, as I have no prior experience with it.
In general, using C# has been a positive experience. The fact that Visual Studio is designed for it (in part) is very helpful at times, because it enables us to utilize the IDE better, and when there is issues, there are often documented ways of solving or troubleshooting them.

Since Unity3D currently only uses C#, finding solutions for Unity with this language is usually very easy (depending on the issue). There are already a lot of forum threads and guides, so less time needs to be spent on "figuring it out".

In addition, the fact that C# is call-by-reference, as well as very type safe is "comforting" to know when programming. For me, it makes it easier to plan and predict code behaviour when programming.

The biggest drawback with C# is the compiler optimization, which can make debugging and troubleshooting harder. When sequencial operations are required, this has to be taken into consideration, which is an annoying factor.

### Process and Communication
We used the repository as our "main access point" for most of the data and content related to the project, which was a decent way of connecting all the material.

We were too slack with the process, even when planning with a flexible model (incremental/SCRUM-ish). We had weekly or biweekly progress discussions, which were very effective. However, with some extra effort and planning, we could have used them more to our advantage over longer time periods. They turned out to be surprisingly motivating for me, because it was easier to see progress, and what needed to be done.

Discussion, talks, and review were done infrequently, mostly at request, or to comment on each others progress/code. This helped with workflow in periods, because we could work uninterrupted. It is likely that it would have helped to have some planned times every day as well, because it can be easy to be stuck in a "I can't solve this, I need to solve this" loop. In these situations, a forced break and discussion could help solve some issues. For future work, I will likely plan to take "frequent" breaks, to allow this situation.


### Version control

We have used Git along with Bitbucket for version control, which has been a positive experience. Separating components/sections into branches has been a good way of separating work and planning for future integration. There were some "negative" situations where some code was replaced in one branch, after already being used by others. However, it allowed us to compare the old and new code, and see how it improved (or not).

### Programming Style
We have not used a very rigid programming style for the project, but we put some focus on using triple-slash commenting. Since we used C#, we have taken basis in Microsoft's own conventions (https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) to some degree, with a focus on our own preferences.
We didn't find it hard to agree on style, but we did discuss the subject briefly. Important to us was frequent commenting, using the same naming style for functions/variables.

For future work, using a more set convention can be helpful to create a more homogenous code. For this project, I would put focus at Microsoft's own convention, along with Unity3D (http://wiki.unity3d.com/index.php/Csharp_Coding_Guidelines). At the same time, it can take time and effort to write in the right style. Personally, I think I would prefer to write the code, and then make corrections afterwards, while reviewing and cleaning up comments.


### Libraries
We have mostly used Tobii's SDK for Unity3D for this project, which made it very simple and easy to import. In addition, Tobii's example code and wiki was a very helpful tool when adapting and writing our own code.

When using libraries, preparing is probably an important step we could have taken more advantage of. While we did read some of the wiki and general guidelines for using Tobii's functionality, a more thorough approach as a team could have helped us getting started faster and more efficiently.

### Approach + review
When preparing for a larger project, we spent time working out and discussing a plan for development. We spent time discussing development models that could suit the project, dependent on the requirements (not much defined = flexible model). In addition, we put focus on how we should work together as a team to complete a common goal.
Luckily, we've had a very relaxed relationship to discussing our own code, so we have been able to openly critique and discuss each others code. However, we should have had more planned code review, as it mostly happened infrequently, and before integration, which could delay it. Separating integration from review could help give us more time, and plan better.

## Individual Discussion
### Good code
For review of good code, I have decided to review EyegazeBrowserPointer.cs. I do not view it as perfect, but as relatively self-explanatory.
It can be found at https://github.com/BaardsethK/ProfProg/blob/master/EyegazeBrowserPointer.cs
The script is a child of EyegazeGeneralPointer.cs (https://github.com/BaardsethK/ProfProg/blob/master/EyegazeGeneralPointer.cs) which I am not yet content with, and needs review/refactor.
In short, is is made to translate the position of a 3D object from world space to a 2D-position relative to the view inside a canvas.

#### Why do I think it is somewhat good.
The code has neccessary commenting, as triple-slash and some explanations on functions.
Most variable names (bar localPos) is self-explanatory, such as mBrowser, mBrowserCanvas, and pointerInBrowserSpace.
The code itself is relatively straight-forward and easy to understand.

However, I am not entirely happy with the whole setup, and would like to rewrite it if I had the time. I would like this space to showcase a better example than listed.

### Bad code
For review of bad code, I have decided to review CircularMotionsDirection.cs (https://github.com/BaardsethK/ProfProg/blob/master/CircularMotionDirection.cs).
In short, the script is used as a handler for when a set of buttons controlling motion is pressed. It uses an enum, set in the Unity Editor, along with a switch to decide where and how fast to move.

#### Name
First of, the name is not very descriptive of the specific action of the code, which is to move the camera along all axes. While I personally understand, it is not clear to others. A more descriptive name like "CameraAxisMovement" or "GazeCameraControls" could be better names, as they describe the functionality of the script.

#### Variable names
The variables could have better naming:
useArea refers to the main canvas in a scene (browser scene: canvas containing browser, etc). This is very unclear from the name, as the canvas containing the buttons is also a use-area. A scene can contain multiple such canvases, but the objective of the script is to navigate the main canvas.

#### FindObject-function
The Awake-function uses Unity's FindObjectWithTag-function. In this case, only one instance of the object should exist in a scene, but it can happen that to exists. It's safer to use a direct reference in the editor through a serialized variable. An option could be to have "if settings == null: FindObjectWithTag" in addition to the direct reference, in case it is forgotten.

#### Switch
The switch consists of many functions copying each other, all doing the same actions with different values. This can be improved by creating a helper function, which handles position update of camera and the canvas containing the controls.
 
In addition, the if can be shortened. motionCircleCanvas.transform.position can be saved as a transform, which can reduce some of the clutter in the code.

### Refactoring
I decided to refactor CircularMotionDirection.cs, after reviewing it.
Refactored code can be found in https://github.com/BaardsethK/ProfProg/blob/master/RefactoredGazeCameraControls.cs

#### Names
Name was changed to GazeCameraControls.cs.
Variable names was updated to be more self-descriptive, mainly using mainUseCanvas.
stdCameraPos was changed to startCameraPos, which is a bit clearer.

#### Load of variables
All variables from the settings-script has been changed.
The mainCameraTransform and gazeCameraControlCanvasTransform are saved as transforms, because it is only the transform that is being affected by the script.
This changes has been done since these are the only values being used/changed.

#### Helper function

A helper function to do Translate on the transforms of mainCamera and GazeCameraControlCanvas has been added. This way, there is less code duplication, and all switch functions can run through the helper function. This is probably not optimal, but cleaner and easier to change than the previous implementation.

#### Conclusion
The code is not shorter, but it has been simplified with the helper function. The variable names should be more descriptive, and it should be easier to understand what the variables refer to by someone looking at the code (given that they understand Unity's transforms and canvases).

####

## Reflection

### Overall
Personally, I feel like professionalism in programming refers to developing and documenting code so it is easily possible for anyone to continue working on it. This way, leaving a project does not burden those who have to continue the work on it. In addition, discussion and refactoring should be a part of this, when possible. This can help create a more homogenous coding/commenting standard across a team, and can make the code easier to understand for someone else.

I feel like it can be summed up in "being able to give your code to someone else without instruction" because this means that your code isn't too hard to learn. The quality of the code is good enough to be understood, the documentation is able to explain it well enough, and the functions "makes sense", even if refactoring can help.

### Code quality
I feel like writing code of as "decent" quality is a requirement for being a professional. For me, this means self explanatory variable names, using the same conventions over a whole program, and a reasonable architecture (however, everything can be justified as "reasonable architecture" to some degree).

### Documentation
Most code should have some documentation, outside of code comments. Most people won't remember everything they did, and the documentation can provide better insight into the architecture/structure. If someone else is take over the project, the documentation can help them understand the structure and code faster, and cut down the time spent learning how the program is stru-ctured and functions.

### Professional Behaviour
Professional behaviour should be a part of professionalism. By this, I mean the ability to act decently towards other programmers, especially if someone new is introduced to a project, or is taking over an old project. It is likely to happen that you will meet someone who knows either more or less than you, that is introduced to a project. 

If they're new, they might ask "dumb" questions because to you, the functionality is obvious. I think it's important to keep in mind that people can have different experience and knowledge from yourself, and that them understanding your way of thinking (and programming) is not a given. 

If they're more experienced, it can be important to be more humble and open about the quality of your own code and documentation. A more experienced programmer might criticize or comment on the code you have written. Being able to take this as an opportunity to improve your own skills, and to reflect on the statements rather than deflect them. It can happen that the programmer can say degrading statements as well, and this is something you can't necessarily change. However, it is something that can be handled in a professional manner.

### Conclusion
For me, professionalism as a programmer is being able to plan that others are going to take over or use your project at some point, and do it well. This can be done by good code quality, and documentation. Both of these are very "open" definitions, so using refactoring, review, and openly discussing with others is important. In addition to planning for others to take over, being able to communicate well with other programmers is important. It allows us to learn from the mistakes of others, and discuss possible solutions or decisions. I believe that being able to combine these factors can help someone develop better code, and become better programmers.
