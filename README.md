# Trivial Uno

A not so trivial implementation of uno. Used as a playground for architecture, the actual uno implementation and strategies are bad.

Components:
- MyUno: this is the actual application. There are also custom cards showing off the customizability.
- TrivialUno.Definitions: All the interfaces and attributes. No dependencies.
- TrivialUno.DefaultCards: All the default Uno cards. Dependent on DefaultEffects and Definitions.
- TrivialUno.DefaultEffects: All effects used by default Uno cards. Dependent on Definitions.
- TrivialUno.Machinery: Implementations for the interfaces the application can use or partially replace.
