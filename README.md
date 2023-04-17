# tree-drawer

Generate images to demonstrate tree construction and manipulation.

Sample of the tree description:

```
@after-each-node frame
@after-last-child next-value frame
@node-size 80 35
a '' '15 5 0'
    b '' '15 5 0'
        end '15 5 0'
        c '' '15 0 5'
            end '5 15 0'
            end '15 0 5'
    end '10 5 5'
```

Some frames of the result:

Frame 5
![05](samples/05.png)

Frame 6
![06](samples/06.png)

Frame 10
![10](samples/10.png)

[Demonstration of the minimax with alphabeta pruning](GraphDrawer/trees/alphabeta.tree)


## Syntax
```
@global-setting values
...
nodeType value1 value2 ... 
    nodeType value1 ...
    @events_list
    ...
    nodeType value1 ...
        nodeType value1 ...
        ...
    ...
```

## Global Settings

```
// Size of node rectangle: width and height
@node-size 80 35

// Minimal spacing between nodes: horizontal and vertical
@node-spacing 15 30

// Minimal distance from image border to any node: horizontal and vertical
@image-margins 100 100

// Order of nodes traversal (and drawing events generation)
@traverse-order dfs

// Events to add after each node was drawn (before its children are processed)
@after-each-node frame

// Events to add after each child subtree was drawn
@after-each-child-subtree frame

// Events to add after last child subtree was drawn
@after-last-child next-value frame
```

## Events

Can be added as @-children of the tree or to global settins:

**frame** - start new animation frame, making copy from the current frame.

**next-value** - change value of its parent node to the next value from values list.
