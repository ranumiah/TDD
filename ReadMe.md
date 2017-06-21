## TDD Cycle

1) **Write a test**. Think about how you would like the operation in your mind to appear in your code. You are writing a story. Invent the interface you wish you had. Include all of the elements in the story that you imagine will be necessary to calculate the right answers.

2) **Make it run**. Quickly getting that bar to go to green dominates everything else. If a clean, simple solution is obvious, then type it in. If the clean, simple solution is obvious but it will take you a minute, then make a note of it and get back to the main problem, which is getting the bar green in seconds. This shift in aesthetics is hard for some experienced software engineers. They only know how to follow the rules of good engineering. Quick green excuses all sins. But only for a moment.

3) **Make it right**. Now that the system is behaving, put the sinful ways of the recent past behind you. Step back onto the straight and narrow path of software righteousness. Remove the duplication that you have introduced, and get to green quickly.

## Dependency and Duplication
Steve Freeman pointed out that the problem with the test and code as it sits is not duplication (which I have not yet pointed out to you, but I promise to as soon as this digression is over). The problem is the dependency between the code and the test�you can't change one without changing the other. Our goal is to be able to write another test that �makes sense� to us, without having to change the code, something that is not possible with the current implementation.
Dependency is the key problem in software development at all scales. If you have details of one vendor's implementation of SQL scattered throughout the code and you decide to change to another vendor, then you will discover that your code is dependent on the database vendor. You can't change the database without changing the code.
If dependency is the problem, duplication is the symptom. Duplication most often takes the form of duplicate logic�the same expression appearing in multiple places in the code. Objects are excellent for abstracting away the duplication of logic.
Unlike most problems in life, where eliminating the symptoms only makes the problem pop up elsewhere in worse form, eliminating duplication in programs eliminates dependency. That's why the second rule appears in TDD. By eliminating duplication before we go on to the next test, we maximize our chance of being able to get the next test running with one and only one change.

## Test Strategies

* Fake It— Return a constant and gradually replace constants with variables until you have the real code.
* Use Obvious Implementation— Type in the real implementation.
* Triangulation - By analogy, when we triangulate, we only generalize code when we have two examples or more. We briefly ignore the duplication between test and model code. When the second example demands a more general solution, then and only then do we generalize.

Triangulation feels funny to me. I use it only when I am completely unsure of how to refactor. If I can see how to eliminate duplication between code and tests and create the general solution, then I just do it. Why would I need to write another test to give me permission to write what I probably could have written in the first place?

However, when the design thoughts just aren't coming, Triangulation provides a chance to think about the problem from a slightly different direction. What axes of variability are you trying to support in your design? Make some of them vary, and the answer may become clearer.

## Value Objects

The pattern for this is Value Object. One of the constraints on Value Objects is that the values of the instance variables of the object never change once they have been set in the constructor.

One implication of Value Objects is that all operations must return a new object, as we saw. Another implication is that Value Objects should implement equals(),



## Random

The dogmatic answer is that we'll wait, not interrupting what we're doing. The answer in my practice is that I will entertain a brief interruption, but only a brief one, and I will never interrupt an interruption (Jim Coplien taught me this rule).

I'm feeling defensive again about taking such teeny-tiny steps. Am I recommending that you actually work this way? No. I'm recommending that you be able to work this way. What I did just now was to work in larger steps and make a stupid mistake halfway through. I unwound a minute's-worth of changes, shifted to a lower gear, and did it over with little steps. I'm feeling better now, so we'll see if we can make the analogous change to Dollar in one swell foop:

This is the kind of tuning you will be doing constantly with TDD. Are the teeny-tiny steps feeling restrictive? Take bigger steps. Are you feeling a little unsure? Take smaller steps. TDD is a steering process—a little this way, a little that way. There is no right step size, now and forever.

Sometimes you have to go backward to go forward, a little like solving a Rubik's Cube.

We could carefully reason about this given our knowledge of the system, but we have clean code and we have tests that give us confidence that the clean code works. Rather than apply minutes of suspect reasoning, we can just ask the computer by making the change and running the tests. In teaching TDD, I see this situation all the time—excellent software engineers spending 5 to 10 minutes reasoning about a question that the computer could answer in 15 seconds. Without the tests you have no choice, you have to reason. With the tests you can decide whether an experiment would answer the question faster. Sometimes you should just ask the computer.