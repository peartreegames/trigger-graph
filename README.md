# TriggerGraph

A Serialized Conditional Reaction Sequence Graph for Unity.

## Installation

Can be installed via the Package Manager > Add Package From Git URL...
`https://github.com/peartreegames/trigger-graph.git`

This repo has an optional dependency on the EvtVariable package which can be installed separately. 
`https://github.com/peartreegames/evt-variables.git`

## Notes

### SerializedReferences

All Nodes are plain C# objects and serialized with SerializedReference.
If you create your own Nodes, be wary, that if you change the name or namespace
it will break the references.

### Prefabs

When editing a Prefab, Unity automatically saves after each change. 
If typing a text box or changing many values, this can be annoying.
It is recommended to disable the AutoSave (checkbox in the top right corner)
when in Prefab Mode.