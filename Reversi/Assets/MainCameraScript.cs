using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {

	private ReversiGame game;
	private GameObject basePlane;
	private GameObject[,] board;
	private const int DIM = 8;
	

	// Use this for initialization
	void Start () {
	
		board = new GameObject[DIM,DIM];
	
		this.transform.position = new Vector3(0,10,0);
		this.transform.LookAt(new Vector3(0,0,0));
		this.transform.localScale = new Vector3(8, 1, 4);
		
		basePlane = (GameObject)Instantiate(Resources.Load("Model_Board"));
		basePlane.transform.localPosition = new Vector3(0.0f,-1.1f,0.0f);

		for( int i = 0; i < DIM; i++ ){
			for( int j = 0; j < DIM; j++ ){
				board[i,j] = GameObject.CreatePrimitive(PrimitiveType.Plane);
				board[i,j].transform.localPosition = new Vector3(convertIndexToCoordinate(i), 0, convertIndexToCoordinate(j) );
				board[i,j].GetComponent<Renderer>().enabled = false;
			}
		}
		
		// Initial Pieces
		board[3,3] = (GameObject)Instantiate(Resources.Load("Model_Piece"));
		board[3,3].transform.localPosition = new Vector3(-0.5f, 0.0f, -0.5f);
		
		board[4,4] = (GameObject)Instantiate(Resources.Load("Model_Piece"));
		board[4,4].transform.localPosition = new Vector3(0.5f, 0.0f, 0.5f);
		
		board[3,4] = (GameObject)Instantiate(Resources.Load("Model_Piece"));
		board[3,4].transform.localPosition = new Vector3(-0.5f, 0.0f, 0.5f);
		board[3,4].transform.Rotate(180,0,0);
		
		board[4,3] = (GameObject)Instantiate(Resources.Load("Model_Piece"));
		board[4,3].transform.localPosition = new Vector3(0.5f, 0.0f, -0.5f);
		board[4,3].transform.Rotate(180,0,0);
		
		
		game = new ReversiGame(DIM);
	}
	
	// Update is called once per frame
	void Update () {
	
		if( Input.GetMouseButtonDown(0) ){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);
			//Debug.Log("This hit at " + hit.point );
			
			int indX = convertCoordinateToIndex(hit.point.x);
			int indY = convertCoordinateToIndex(hit.point.z);
			
			Debug.Log( indX + ", " + indY);
			
			if( game.isValidMove( indX, indY ) ){
				addPieceToBoard( game.getTurn(), indX, indY );
				game.endTurn();
				Debug.Log("Is a valid move.");
			}
			else{
				Debug.Log("Not a valid move.");
			}
		}
		if( Input.GetMouseButtonDown(1) ){
			//Debug.Log("Right Clicked");
		}
	}
	
	private float convertIndexToCoordinate( int x ){
		return ((float)(x)) - 4.0f +0.5f;
	}

	private int convertCoordinateToIndex( float x ){
		return Mathf.FloorToInt(x) + 4;
	}
	
	private void addPieceToBoard( int player, int x, int y ){
		
		if( x < 0 || x >= DIM || y < 0 || y >= DIM ){
			return;
		}
		
		if( player == ReversiGame.BLACK || player == ReversiGame.WHITE ){
		
			board[y,x] = (GameObject)Instantiate(Resources.Load("Model_Piece"));
			board[y,x].transform.localPosition = new Vector3(convertIndexToCoordinate(x), 0.0f, convertIndexToCoordinate(y));
			game.setPiece( x, y, player );
			
			if( player == ReversiGame.WHITE ){
				board[y,x].transform.Rotate(180,0,0);
			}
		}
	}
}

public sealed class ReversiGame{
	
	private int[,] boardInt;
	private int dim;
	
	public const int BLACK = 1;
	public const int WHITE = -1;

	private int turn;
	private int notTurn;
	
	public ReversiGame( int dimension ){
		
		this.dim = dimension;
		boardInt = new int[dim,dim];
		this.turn = BLACK;
		this.notTurn = WHITE;
		
		for( int i = 0; i < dim; i++ ){
			for( int j = 0; j < dim; j++ ){
				boardInt[i,j] = 0;
			}
		}
		
		boardInt[3,3] = BLACK;
		boardInt[3,4] = WHITE;
		boardInt[4,3] = WHITE;
		boardInt[4,4] = BLACK;
	}
	
	public int getTurn(){
		return this.turn;
	}
	
	public void endTurn(){
		
		this.notTurn = this.turn;
		
		if( this.turn == BLACK ){
			this.turn = WHITE;
		}
		else{
			this.turn = BLACK;
		}
	}
	
	public bool isValidMove( int x, int y ){
		
		if( x < 0 || x >= dim || y < 0 || y >= dim ){
			return false;
		}
		
		if( makesLineHorizOrVert( x, y )){
			return true;
		}
		
		//if( boardInt[y,x] == 0 ){
		//	return true;
		//}
		return false;
	}
	
	public void setPiece( int x, int y, int player ){
		boardInt[y,x] = player;
	}
	
	private bool makesLineHorizOrVert( int x, int y ){
		
		int i;
		int count;
		
		// Horizontal Positive
		for( i = x+1, count = 0; i < this.dim; i++ ){
			if( boardInt[y,i] == this.turn ){
				if( count > 0 ){
					return true;
				}
				else{
					break;
				}
			}
			else if( boardInt[y,i] == 0 ){
				break;
			}
			count++;
		}
		
		// Horizontal Negative
		for( i = x-1, count = 0; i >= 0; i-- ){
			if( boardInt[y,i] == this.turn ){
				if( count > 0 ){
					return true;
				}
				else{
					break;
				}
			}
			else if( boardInt[y,i] == 0 ){
				break;
			}
			count++;
		}
		
		// Vertical Positive
		for( i = y+1, count = 0; i < this.dim; i++ ){
			if( boardInt[i,x] == this.turn ){
				if( count > 0 ){
					return true;
				}
				else{
					break;
				}
			}
			else if( boardInt[i,x] == 0 ){
				break;
			}
			count++;
		}
		
		// Vertical Negative
		for( i = y-1, count = 0; i >= 0; i-- ){
			if( boardInt[i,x] == this.turn ){
				if( count > 0 ){
					return true;
				}
				else{
					break;
				}
			}
			else if( boardInt[i,x] == 0 ){
				break;
			}
			count++;
		}
		return false;
	}
	
}

