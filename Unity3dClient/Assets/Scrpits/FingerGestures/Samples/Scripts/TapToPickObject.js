var textField : TextMesh;

function OnEnable()
{
    // register to finger down event
    FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
}

function OnDisable()
{
    // unregister from finger down event
    FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
}

function FingerGestures_OnFingerDown( fingerIndex : int, fingerPos : Vector2 )
{
    var obj : GameObject = PickObject( fingerPos );

    if( obj )
        DisplayText( "You pressed " + obj.name );
    else
        DisplayText( "You didn't pressed any object" );
}

function DisplayText( text )
{
    if( textField )
        textField.text = text;
    else
        Debug.Log( text );
}

function PickObject( screenPos : Vector2 ) : GameObject
{
    var ray : Ray = Camera.main.ScreenPointToRay( screenPos );
    var hit : RaycastHit;

    if( Physics.Raycast( ray, hit ) )
        return hit.collider.gameObject;

    return null;
}
