# Lighthaus Interview Performance Task
## How to modify blood flow path
1. Select the BloodSpline object in the Hierarchy.
2. Click a node along the spline (this selects the point on the spline).
3. Drag the transform handle to move the point.

### Other options
Looping - 
You can make the Bezier spline loop around by clicking the loop bool. 
Unclicking the loop bool will remove the loop but leaves the end point in the same location as the start point.

Adding more points -
You can add more points to the spline by clicking the "Add a curve" button.

Changing handle modes - 
You can change the behavior of the handles by selecting a point and changing the mode enum

a. Free - Not constrained by any other handle.

b. Mirrored - Handles on either side of the point are mirrored.

c. Aligned - Like mirrored but the tangent is normalized.

## How to modify the color and speed of the blood flow
1. Select the BloodSpline object in the Hierarchy.
2. On the `BloodFlowController` component you can modify the `Blood Flow Color` field and it will update the material that all the blood cells use.
3. Also on the `BloodFlowController` component you can modify the speed of the flow with the `Lap Duration` field. The lower the lap duration the faster the flow.
