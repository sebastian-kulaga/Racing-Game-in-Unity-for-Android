#pragma strict


var meshrenders:GameObject[];
var count:int;
var rotationSpeed:float;
function Start () {

}

function Update () {


if(Input.GetKeyDown(KeyCode.Space)  )
{
 
   count++; 
   if( count <0 || count > meshrenders.Length-1 ) count=0;
   hideAllThenEnableOne();
  
   
   
}

else if(   Input.GetKeyDown(KeyCode.LeftArrow ))
{
 
 
   
   rotationSpeed*=-1;
  
}
 

 transform.Rotate(0,rotationSpeed*Time.deltaTime,0 );
 
 
 //speed
 
 if(   Input.GetKey (KeyCode.LeftArrow ))
 {
  rotationSpeed--;
 
 }
else if(  Input.GetKey (KeyCode.RightArrow) )
{
rotationSpeed++;
}
else  rotationSpeed=4;
}


var textureString:String = "Apply Second Texture ";
var  secondTextures:Texture[];

function OnGUI()
{
     
   if( GUI.Button( Rect((Screen.width/2)-90,Screen.height-40,180,40) ,"Next Car Model :  "+(count+1) ))
   {
   
   count++; 
   if( count <0 || count > meshrenders.Length-1 ) count=0;
   hideAllThenEnableOne();
   }
   
   if( GUI.Button( Rect((Screen.width/2)-90,Screen.height-8820,180,80) ,textureString ))
   {
   
       meshrenders[count].SetActiveRecursively(true);
   }

  

 

}







function hideAllThenEnableOne()
{

  for( var i:int=0;i<=meshrenders.Length-1;i++)
  {
  
   meshrenders[i].SetActiveRecursively(false);
  
  }
  
   meshrenders[count].SetActiveRecursively(true);



}