
;;;==============================================
;;;												*
;;; Custom file for the ComS 437 Tutorial Demo  *
;;;												*
;;;==============================================

(deftemplate player
	(multislot name))

(deftemplate monster 
	(multislot name) 
	(field type)
	(field id))

(deftemplate weapon
	(multislot name)
	(field type)
	(field strong-against))
	
(defrule startup
	(initial-fact)
	=>
	(assert
		(monster (name Salamander) (type fire) (id 0))
		(monster (name Tortoise) (type earth) (id 1))
		(monster (name Eagle) (type wind) (id 2))
		(monster (name Shark) (type water) (id 3))
		(monster (name Thunder Rat) (type electricity) (id 4))
		(weapon (name Flaming Arrow) (type fire) (strong-against 2))
		(weapon (name Giant Rock) (type earth) (strong-against 4))
		(weapon (name Water Balloon) (type water) (strong-against 0))
		(weapon (name Fan) (type wind) (strong-against 1))
		(weapon (name Stun Gun) (type electricity) (strong-against 3)))

	(seed (round (time)))
		
	(printout t "Please enter your name." crlf)
	(bind $?pID (readline))
	
	(assert (player (name $?pID)))
	
	(printout t crlf "Hello " $?pID crlf)
	
	(printout t "Would you like to hunt monsters? (yes/no)" crlf)
	(bind ?answer (read))
	(printout t crlf crlf)
	
	(assert (answer ?answer))
)

(defrule game-prompt
	
		?pID <- (prompt ?p)
	=>
		(retract ?pID)
		(printout t "Would you like to hunt again? (yes/no)" crlf)
		(bind ?answer (read))
		(printout t crlf crlf)	
		(assert (answer ?answer))		
)

(defrule game-loop-success
	
		?aID <- (answer ?answer)
		(test (= (str-compare "yes" ?answer) 0))
	=>
		(retract ?aID)
		(assert (hunt yes))
		;(printout t "Game Loop Success" crlf)
)

(defrule game-loop-fail
	
		?aID <- (answer ?answer)
		(test (neq (str-compare "yes" ?answer) 0))
		(player  (name $?pName))
	=>
		(retract ?aID)
		(printout t "Goodbye, " $?pName "!" crlf)
)

(defrule go-hunting

		?hID <- (hunt ?h)
	=>
		(retract ?hID)
		(assert (target (random 0 4)))
		;(printout t "Going Hunting" crlf)
)

(defrule face-target

		?tID <- (target ?t)
		(monster (name $?mName) (type ?mType) (id ?t))
		(weapon  (name $?wName) (type ?wType) (strong-against ?t))
		(player  (name $?pName))
	=>
		(retract ?tID)
		
		(printout t $?pName " used the " ?wType " type weapon " $?wName " to defeat the " ?mType " type monster " $?mName "." crlf crlf)
		(assert (prompt yes))
)
		