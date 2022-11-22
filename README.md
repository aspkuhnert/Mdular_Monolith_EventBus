Diese Solutions sind Teil meiner Arbeit als Softwareentwicklerin bei CTO Software GmbH. Mit diesen Tutorials wird das Ziel verfolgt, Entwickler/innen in diesem Unternehmen in die von mir vorangetriebene Weiter- bzw. Neuentwicklung einzuführen und darüber hinaus anzuleiten. Dieses Tutorial ist work-in-progress (Nov. 2022).

Der Quellcode dieses Repository entspricht dem aus https://github.com/aspkuhnert/Microservices_EventBus_Outbox. Diesmal ist es allerdings keine Lösung als Microservices, sondern eine als getrennte Module -als "Modular Monolith"- mit "In Memory Eventbus" und ohne Container. Auf diese Weise sollen die (technischen) Unterschiede zwischen Microservices und einem "Modular Monolith" dargestellt werden. Im wesentlichen hat ein Monolith lediglich 1 API-Projekt und die Module werden alle im globalen DI-Container miteinander "gekoppelt".

Was ich aber gerne betonen möchte ist, dass weder Microservices noch der "Modular Monolith" als "silver bullet" zur Lösung alle Probleme einer z.B. über 10 oder 20 Jahre gewachsenen Software gesehen werden sollten. Technisch ist beides (vergleichsweise problemlos) lösbar (nicht nur in .NET), wobei gerade Microservices richtig umgesetzt einen ordentlichen Umfang auf technischer Ebene bekommen können. Dinge wie 
- Service Mesh
- Saga Pattern
- tatsächlich verteiltes Deployment auf untersch. Servern
- Sicherheit

(um nur einiges zu nennen) erhöhen die Komplexität der Umsetzung einer Software in Form von Microservices enorm. Das sollte nie vergessen werden.

Die große Herausforderungen bei Microservices, aber auch beim "Modular Monolith", liegt m.E. ganz woanders: in der Modularisierung der Businesslogik und zwar so, dass es kein kompakter Monolith mehr wird.
