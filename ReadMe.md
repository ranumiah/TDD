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

We can't mark our test for $5 + $5 done until we've removed all of the duplication. We don't have code duplication, but we do have data duplication—the $10 in the fake implementation:

Before when we've had a fake implementation, it has been obvious how to work backward to the real implementation. It simply has been a matter of replacing constants with variables. This time, however, it's not obvious to me how to work backward. So, even though it feels a little speculative, we'll work forward.

You will likely end up with about the same number of lines of test code as model code when implementing TDD. For TDD to make economic sense, you'll need to be able to either write twice as many lines per day as before, or write half as many lines for the same functionality. You'll have to measure and see what effect TDD has on your own practice. Be sure to factor debugging, integrating, and explaining time into your metrics, though.

This test is a little ugly, because it is testing the guts of the implementation, not the externally visible behavior of the objects. However, it will drive us to make the changes we need to make, and this is only an experiment, after all. Here is the code we would have to modify to make it work:

There is no obvious, clean way (not to me, anyway; I'm sure you could think of something) to check the currency of the argument if and only if it is a Money. The experiment fails, we delete the test (which we didn't like much anyway), and away we go.

## Bullets/Summary

* Made a list of the tests we knew we needed to have working
* Told a story with a snippet of code about how we wanted to view one operation
* Ignored the details of JUnit for the moment
* Made the test compile with stubs
* Made the test run by committing horrible sins
* Gradually generalized the working code, replacing constants with variables
* Added items to our to-do list rather than addressing them all at once
* Translated a design objection (side effects) into a test case that failed because of the objection
* Got the code to compile quickly with a stub implementation
* Made the test work by typing in what seemed to be the right code
* Noticed that our design pattern (Value Object) implied an operation
* Tested for that operation
* Implemented it simply
* Didn't refactor immediately, but instead tested further
* Refactored to capture the two cases at once
* Used functionality just developed to improve a test
* Noticed that if two tests fail at once we're sunk
* Proceeded in spite of the risk
* Used new functionality in the object under test to reduce coupling between the tests and the code
* Couldn't tackle a big test, so we invented a small test that represented progress
* Wrote the test by shamelessly duplicating and editing
* Even worse, made the test work by copying and editing model code wholesale
* Promised ourselves we wouldn't go home until the duplication was gone
* Stepwise moved common code from one class (Dollar) to a superclass (Money)
* Made a second class (Franc) a subclass also
* Reconciled two implementations—equals()—before eliminating the redundant one
* Took an objection that was bothering us and turned it into a test
* Made the test run a reasonable, but not perfect way—getClass()
* Decided not to introduce more design until we had a better motivation
* Took a step toward eliminating duplication by reconciling the signatures of two variants of the same method—times()
* Moved at least a declaration of the method to the common superclass
* Decoupled test code from the existence of concrete subclasses by introducing factory methods
* Noticed that when the subclasses disappear some tests will be redundant, but took no action
* Were a little stuck on big design ideas, so we worked on something small we noticed earlier
* Reconciled the two constructors by moving the variation to the caller (the factory method)
* Interrupted a refactoring for a little twist, using the factory method in times()
* Repeated an analogous refactoring (doing to Dollar what we just did to Franc) in one big step
* Pushed up the identical constructors
* Reconciled two methods—times()—by first inlining the methods they called and then replacing constants with variables
* Wrote a toString() without a test just to help us debug
* Tried a change (returning Money instead of Franc) and let the tests tell us whether it worked
* Backed out an experiment and wrote another test. Making the test work made the experiment work
* Finished gutting subclasses and deleted them
* Eliminated tests that made sense with the old code structure but were redundant with the new code structure
* Reduced a big test to a smaller test that represented progress ($5 + 10 CHF to $5 + $5)
* Thought carefully about the possible metaphors for our computation
* Rewrote our previous test based on our new metaphor
* Got the test to compile quickly
* Made it run
* Looked forward with a bit of trepidation to the refactoring necessary to make the implementation real
* Didn't mark a test as done because the duplication had not been eliminated
* Worked forward instead of backward to realize the implementation
* Wrote a test to force the creation of an object we expected to need later (Sum)
* Started implementing faster (the Sum constructor)
* Implemented code with casts in one place, then moved the code where it belonged once the tests were running
* Introduced polymorphism to eliminate explicit class checking
* Added a parameter, in seconds, that we expected we would need
* Factored out the data duplication between code and tests
* Wrote a test (testArrayEquals) to check an assumption about the operation of Java
* Introduced a private helper class without distinct tests of its own
* Made a mistake in a refactoring and chose to forge ahead, writing another test to isolate the problem
* Wrote the test we wanted, then backed off to make it achievable in one step
* Generalized (used a more abstract declaration) from the leaves back to the root (the test case)
* Followed the compiler when we made a change (Expression fiveBucks), which caused changes to ripple (added plus() to Expression, and so on)
* Wrote a test with future readers in mind
* Suggested an experiment comparing TDD with your current programming style
* Once again had changes of declarations ripple through the system, and once again followed the compiler's advice to fix them
* Tried a brief experiment, then discarded it when it didn't work out


## WHAT'S NEXT?
Is the code finished?  I don't believe in “finished.” TDD can be used as a way to strive for perfection, but that isn't its most effective use. If you have a big system, then the parts that you touch all the time should be absolutely rock solid, so you can make daily changes confidently. As you drift out to the periphery of the system, to parts that don't change often, the tests can be spottier and the design uglier without interfering with your confidence.

When I've done all of the obvious tasks, I like running a code critic, like Small-Lint for Smalltalk. Many of the suggestions that come up I already know about, or I disagree with. Automated critics don't forget, however, so if I don't delete an obsolete implementation I don't have to stress. The critic will point it out.

Another “what's next?” question is, “What additional tests do I need?” Sometimes you think of a test that “shouldn't” work, and it does. Then you need to find out why. Sometimes a test that shouldn't work really doesn't, and you can record it as a known limitation or as work to be done later.

Finally, when the list is empty is a good time to review the design. Do the words and concepts play together? Is there duplication that is difficult to eliminate given the current design? (Lingering duplication is a symptom of latent design.)


## TEST QUALITY
The tests that are a natural by-product of TDD are certainly useful enough to keep running as long as the system is running. Don't expect them to replace the other types of testing:

* Performance
* Stress
* Usability

However, if the defect density of test-driven code is low enough, then the role of professional testing will inevitably change from “adult supervision” to something more closely resembling an amplifier for the communication between those who generally have a feeling for what the system should do and those who will make it do. As a stand-in for a long and interesting conversation about the future of professional testing, here are a couple of widely shared measurements of the tests written above.

Statement coverage certainly is not a sufficient measure of test quality, but it is a starting place. TDD followed religiously should result in 100 percent statement coverage. JProbe (www.sitraka.com/software/jprobe) reports only one line in one method not covered by the test cases—Money.toString(), which we added explicitly as a debugging aid, not real model code.

Defect insertion is another way of evaluating test quality. The idea is simple: change the meaning of a line of code and a test should break. You can do this manually, or with a tool such as Jester (jester.sourceforge.net). Jester reports only one line it is able to change without breaking, Pair.hashCode(). We faked the implementation to just return 0. Returning a different constant doesn't actually change the meaning of the program (one fake number is as good as another), so it isn't really a defect that has been inserted.

Phlip, one of my reviewers, made a point about test coverage that bears repeating here. A gross measure of coverage is the number of tests testing different aspects of a program divided by the number of aspects that need testing (the complexity of the logic). One way to improve coverage is to write more tests, hence the dramatic difference in the number of tests a test-driven developer would write for code and the number of tests a professional tester would write. (Chapter 32) gives details of an example in which I wrote 6 tests and a tester wrote 65 tests for the same problem.) However, another way to improve coverage is to take a fixed set of tests and simplify the logic of the program. The refactoring step often has this effect—conditionals replaced by messages, or by nothing at all. In Phlip's words, “Instead of increasing the test coverage to walk all permutations of input (more properly an efficiently reduced sample of all possible permutations), we just leave the same tests covering various permutations of code as it shrinks.”

## ONE LAST REVIEW
The three items that come up time and again as surprises when teaching TDD are:

* The three approaches to making a test work cleanly—fake it, triangulation, and obvious implementation
* Removing duplication between test and code as a way to drive the design
* The ability to control the gap between tests to increase traction when the road gets slippery and cruise faster when conditions are clear

## QUESTIONS?!
What if I got to rewrite everything I ever wrote 20 times? Would I keep finding insight and surprise every time? Is there some way to be more mindful as I program so I can squeeze all the insight out of the first three times? The first time?