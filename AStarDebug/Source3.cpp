
// A C++ Program to implement A* Search Algorithm
#include<bits/stdc++.h>
#include <omp.h>
#include <stdio.h>
#include <semaphore.h>
#include <mutex> 
using namespace std;


// To shut the terminal up with "unused varibels". Unused variables are normal
//if there are some selective defines//
#pragma GCC diagnostic ignored "-Wunused-but-set-variable"


//---------------------------DEFINES----------------------------------------------------------------//


//-------------NUMBER OF THREADS & DELAY(LOAD)----------------------------//

//#define DEBUG_NUM_OF_TD 1
#define LOAD 0

//------------------------------------------------------------------------//


//-----------ENABLE/DISABLE FUNTION FAILURE PRINT-------------------------//

#define PRINT_ISVALID_FAILURE
#define PRINT_ISUNBLOCKED_FAILURE
#define PRINT_ISDESTINATION_FAILURE

//------------------------------------------------------------------------//


//-----------ENABLE/DISABLE FOUND PATH PRINT------------------------------//

#define PRINT_FOUND_PATH

//------------------------------------------------------------------------//


//-----------ENABLE/DISABLE BASIC DEBUG PRINT-----------------------------//

#define PRINT_OPEN_LIST_BEFORE_PARALLEL
#define PRINT_CLOSED_LIST_BEFORE_PARALLEL
#define DEBUG_MESSAGES_SEVERITY_ALL
#define PRINT_CURRENT_CELL

//-----------ENABLE/DISABLE TIME PERFORMING PRINT-------------------------//

#define PRINT_THE_WHOLE_PROGRAM_PERFORMING_TIME
//#define PRINT_ASTARTSEARCH_TIME_PERFORMING
//#define PRINT_NORTH_SECTION_PERFORMING_TIME
//#define PRINT_SOUTH_SECTION_PERFORMING_TIME
//#define PRINT_EAST_SECTION_PERFORMING_TIME
//#define PRINT_WEST_SECTION_PERFORMING_TIME

//------------------------------------------------------------------------//


//-----------ENABLE/DISABLE CRITICAL SECTION OPENMP-----------------------//

#define CRITICAL_SECTIONS

//------------------------------------------------------------------------//


//-----------SET TXT LOCATION FROM CPP OR CMD-----------------------------//

//#define IDE_USING
#define CMD_Linux

//------------------------------------------------------------------------//


//-----------PROBLEM INSTANCE AND WIDTH-----------------------------------//

#define DEBUG_PRBL_WIDE 4
//#define DEBUG_PATH_MTRX "./test_bench/matrix50x50.txt" //Used with IDE_USING

//------------------------------------------------------------------------//



//--------------------------------------------------------------------------------------------------//




//---------------------------GLOBAL VARIABLES-------------------------------------------------------//

// Value of column/row
int col_row = 0;
// Creating a shortcut for int, int pair type 
typedef pair<int, int> Pair;
// Creating a shortcut for pair<int, pair<int, int>> type 
typedef pair<double, pair<int, int>> pPair;

//-------------------------------------------------------------------------------------------------//




//--------------------------STRUCTURES-------------------------------------------------------------//

// A structure to hold the neccesary parameters 
struct cell
{
	// Row and Column index of its parent 
	int parent_i;
	int parent_j;
	double f;
};

// A structure to make a workshare manually
struct sections_per_thread
{
	int first_section;
	int second_section;
	int third_section;
	int fourth_section;
}thread_arrangment;

//------------------------------------------------------------------------------------------------//


// A Utility Function to check whether given cell (row, col) 
// is a valid cell or not. 
bool isValid(int row, int col)
{
	//printf("row = %d\n",row);
	//printf("col = %d\n",col);
	// Returns true if row number and column number 
	// is in range
	return (row >= 0) && (row < col_row) &&
		(col >= 0) && (col < col_row);
}

// A Utility Function to check whether the given cell is 
// blocked or not 
bool isUnBlocked(vector<vector<int>> grid, int row, int col)
{
	//
	//
	// Returns true if the cell is not blocked else false 
	if (grid[row][col] == 0)
		return (true);
	else
		return (false);
}

// A Utility Function to check whether destination cell has 
// been reached or not 

bool isDestination(int col, int row, Pair dest)
{
	if (row == dest.first && col == dest.second) {
		return (true);
	}
	else
	{
		return (false);
	}
}

// A Utility Function to trace the path from the source 
// to destination 
void tracePath(vector<vector<cell>> cellDetails, vector<vector<char>> output_file_map, Pair dest, Pair src)
{
	int col = dest.first;
	int row = dest.second;

	stack<Pair> Path;

	while (!(cellDetails[row][col].parent_i == row
		&& cellDetails[row][col].parent_j == col))
	{
		output_file_map[row][col] = 'x';
		Path.push(make_pair(row, col));
		int temp_row = cellDetails[row][col].parent_i;
		int temp_col = cellDetails[row][col].parent_j;
		row = temp_row;
		col = temp_col;

		if (src.second == row && src.first == col)
			break;
	}

	output_file_map[dest.second][dest.first] = 'E';
	Path.push(make_pair(row, col));
	while (!Path.empty())
	{
		pair<int, int> p = Path.top();
		Path.pop();

#ifdef PRINT_FOUND_PATH
		printf("-> (%d,%d) ", p.first, p.second);
#endif // PRINT_FOUND_PATH
	}

#ifdef PRINT_FOUND_PATH
	printf("\n");
#endif // PRINT_FOUND_PATH

	std::ofstream outfile;
	outfile.open("res.txt", std::ios_base::out);
	for (int i = 0; i < col_row; i++)
	{
		for (int j = 0; j < col_row; j++)
		{
			outfile << output_file_map[i][j];
		}
		outfile << endl;
	}

	return;
}

// A Function to find the shortest path between 
// a given source cell to a destination cell according 
// to A* Search Algorithm 
void aStarSearch(vector<vector<int>> grid, Pair src, Pair dest, int threads)
{
	sem_t sem;
	std::mutex CellAcess;
	std::mutex openListAccess;
	std::mutex payloadTransition; // payload = parent_i,parent_j,f
	double startaStarSearch;
	double endaStarSearch;
	startaStarSearch = omp_get_wtime();

	//Static debug messages
	//printf("src.second = %d\n",src.second);
	//printf("src.first  = %d\n",src.first);

	// If the source is out of range 
	if (isValid(src.second, src.first) == false)
	{
#ifdef PRINT_ISVALID_FAILURE
		printf("Source is invalid\n");
#endif // PRINT_ISVALID_FAILURE
		return;
	}

	// If the destination is out of range 
	if (isValid(dest.second, dest.first) == false)
	{
#ifdef PRINT_ISVALID_FAILURE
		printf("Destination is invalid\n");
#endif // PRINT_ISVALID_FAILURE
		return;
	}

	// Either the source or the destination is blocked 
	if (isUnBlocked(grid, src.second, src.first) == false || isUnBlocked(grid, dest.second, dest.first) == false)
	{
#ifdef PRINT_ISUNBLOCKED_FAILURE
		printf("Source or the destination is blocked\n");
#endif // PRINT_ISUNBLOCKED_FAILURE
		return;
	}

	// If the destination cell is the same as source cell 
	if (isDestination(src.first, src.second, dest) == true)
	{
#ifdef PRINT_ISDESTINATION_FAILURE
		printf("We are already at the destination\n");
#endif // PRINT_ISDESTINATION_FAILURE
		return;
	}

	// output_file_map vector saves parsed matrix that represent the originial input
	vector<vector<char>> output_file_map(col_row, vector<char>(col_row, ' '));
	for (int i = 0; i < col_row; i++)
	{
		for (int j = 0; j < col_row; j++)
		{
			if (i == src.second && j == src.first)
				output_file_map[i][j] = 'S';
			if (i == dest.second && j == dest.first)
				output_file_map[i][j] = 'E';
			if (grid[i][j] == 1)
				output_file_map[i][j] = '1';
		}
	}

	/*
	SHORT DESCRIPTION (PURPOSE):
		closeList saves cells/fields that are visited during the searching.
		If the cell/field is in closedList it means the algorithm can't
		visit this cell again.

	FULL DESCRIPTION (STRUCTURE):
		The size of the matrix = col_row x col_row
		//[AK]: 04/12/2020: [DEBUG_PRBL_WIDE][DEBUG_PRBL_WIDE] is only for visual studio version of code!!
							Linux code: bool closedList[col_row][col_row];
	*/
	bool closedList[col_row][col_row];
	memset(closedList, false, sizeof(closedList));

	/*
	SHORT DESCRIPTION (PURPOSE):
		Declare a 2D array of structure to hold the details
		of that cell
	FULL DESCRIPTION (STRUCTURE):
		cellDetails vector contains:
		1. parent_i = row of the parent cell
		2. parent_j = col of the parent cell
		3. f = weigth function
		4. g = f
	*/

	vector<vector<cell> > cellDetails;//(col_row*col_row, vector<cell>(col_row*col_row));

	//Old version of creating vector of vector. It does not work with bigger col_row
	//vector<vector<cell> > cellDetails(col_row*col_row, vector<cell>(col_row*col_row));

	/*
	Set fields for each of the fields
	f = g = Maximum type value
	parent_i = parent_j = -1 (default value)
	*/
	int i, j;

	for (i = 0; i < col_row; i++) {
		// Vector to store column elements 
		vector<cell> column;

		for (j = 0; j < col_row; j++) {
			column.push_back({ -1,-1,FLT_MAX });
		}

		// Pushing back above 1D vector 
		// to create the 2D vector 
		cellDetails.push_back(column);
	}

	/* Old version of Initialization vector of vector.
	for (i = 0; i < col_row; i++)
	{
		for (j = 0; j < col_row; j++)
		{
			cellDetails[i][j].f = FLT_MAX; // care, it was FLT_MAX
			cellDetails[i][j].g = FLT_MAX; // care, it was FLT_MAX
			cellDetails[i][j].parent_i = -1;
			cellDetails[i][j].parent_j = -1;
		}
	}
*/
// Initialising the parameters of the starting node 
	i = src.second, j = src.first;
	cellDetails[i][j].f = 0.0;
	cellDetails[i][j].parent_i = i;
	cellDetails[i][j].parent_j = j;

	/*
	SHORT DESCRIPTION (PURPOSE):
		openList saves Temporary Cells (TCs). Capacity of TCS is 4.
		4 = (North, South, East, West).
		Temporary cells will be visited in the future.

	FULL DESCRIPTION (STRUCTURE):
		Create an open list having information as-
		<f, <i, j>>
		f = g,
		<i,j> = <row, col>
		Note that 0 <= i <= ROW-1 & 0 <= j <= COL-1
		This open list is implenented as a set of pair of pair.*/
	set<pPair> openList;
	// Put the starting cell on the open list and set its 
	// 'f' as 0 
	openList.insert(make_pair(0.0, make_pair(i, j)));
	//Semaphore
	//sem_t *sem - Specifies the semaphore to be initialized
	//0          - Means it is shared between threads
	//
	sem_init(&sem, 0, 1);

	int value = 0;

	// We set this boolean value as false as initially 
	// the destination is not reached. 
	bool foundDest = false;
	int tid;
	int nthreads;
	omp_set_num_threads(threads);

	//Hardcoded thread arrangmend
	switch (threads) {
	case 1:
		//Single core
		printf("Single core\n");
		thread_arrangment.first_section = 0;
		thread_arrangment.second_section = 0;
		thread_arrangment.third_section = 0;
		thread_arrangment.fourth_section = 0;
		break;
	case 2:
		//Two cores
		printf("Two Cores\n");
		thread_arrangment.first_section = 0;
		thread_arrangment.second_section = 0;
		thread_arrangment.third_section = 1;
		thread_arrangment.fourth_section = 1;
		break;
	case 3:
		//Three cores
		printf("Three cores\n");
		thread_arrangment.first_section = 0;
		thread_arrangment.second_section = 1;
		thread_arrangment.third_section = 2;
		thread_arrangment.fourth_section = 0;
		break;
	case 4:
		//Four cores
		printf("Four cores\n");
		thread_arrangment.first_section = 0;
		thread_arrangment.second_section = 1;
		thread_arrangment.third_section = 2;
		thread_arrangment.fourth_section = 3;
		break;
	case 5:
		//Five cores
		printf("Five cores\n");
		thread_arrangment.first_section = 0;
		thread_arrangment.second_section = 1;
		thread_arrangment.third_section = 2;
		thread_arrangment.fourth_section = 3;
		break;
	case 6:
		//Six cores
		printf("Six cores\n");
		thread_arrangment.first_section = 0;
		thread_arrangment.second_section = 1;
		thread_arrangment.third_section = 2;
		thread_arrangment.fourth_section = 3;
		break;
	default:
		cout << "Invalid number of cores" << endl;
	}
	pPair p;
	int firstTimeCheck = 0;
	// To store 'f' of the 4 succesors
	double fNew;
	std::set<pPair>::iterator it;
#pragma omp parallel default(none) private(i,j,firstTimeCheck,fNew,tid,it) shared(CellAcess,payloadTransition,openListAccess,p,value,sem,thread_arrangment,col_row,openList,nthreads,foundDest,cellDetails,dest,src,output_file_map,closedList,grid)
	{
		firstTimeCheck = 0;
		tid = omp_get_thread_num();

#pragma omp critical
		{
			sem_getvalue(&sem, &value);
		}


		/*dev*/		//DEBUG
		/*dev*/		if (tid == 0)
			/*dev*/ {
			/*dev*/            printf("SECTION = none THREAD = %d VALUE = %d : Waiting for free semaphore\n", tid, value);
			/*dev*/
		}
		/*dev*/	    else
			/*dev*/ {
			/*dev*/	        printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Waiting for free semaphore\n", tid, value);
			/*dev*/
		}
		/*dev*/	    //END DEBUG


		sem_wait(&sem);
		/*dev*/        printf("New thread has been staretd working\n");
#pragma omp critical
		{
			sem_getvalue(&sem, &value);
		}

		/*dev*/        //DEBUG
		/*dev*/        if (tid == 0)
			/*dev*/ {
			/*dev*/            printf("SECTION = none THREAD = %d VALUE = %d : Went through the semaphpre\n", tid, value);
			/*dev*/
		}
		/*dev*/	    else
			/*dev*/ {
			/*dev*/	        printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Went through the semaphore\n", tid, value);
			/*dev*/
		}
		/*dev*/	    //END DEBUG


		while (!openList.empty())
		{
			//if(foundDest == 0)
			//{
			/*dev*/			sem_getvalue(&sem, &value);

			/*dev*/			//DEBUG
			/*dev*/			if (tid == 0)
				/*dev*/ {
				/*dev*/             printf("SECTION = none THREAD = %d VALUE = %d : Went through while loop\n", tid, value);
				/*dev*/
			}
			/*dev*/	        else
				/*dev*/ {
				/*dev*/	        printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Went through while loop\n", tid, value);
				/*dev*/
			}
			/*dev*/	        //END DEBUG

			/*dev*/	        //sem_wait( &sem);

			if (firstTimeCheck > 0)
			{
				/*dev*/			    sem_getvalue(&sem, &value);

				/*dev*/			    //DEBUG
				/*dev*/			    if (tid == 0)
					/*dev*/ {
					/*dev*/                    printf("SECTION = none THREAD = %d VALUE = %d : It's not first time in the while loop\n", tid, value);
					/*dev*/
				}
				/*dev*/	            else
					/*dev*/ {
					/*dev*/	                printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : It's not first time in the while loop\n", tid, value);
					/*dev*/
				}
				/*dev*/			    //END DEBUG


				sem_wait(&sem);

				/*dev*/			    sem_getvalue(&sem, &value);

				/*dev*/			    //DEBUG
				/*dev*/			    if (tid == 0)
					/*dev*/ {
					/*dev*/                    printf("SECTION = none THREAD = %d VALUE = %d : Ending firstTimeCheck if\n", tid, value);
					/*dev*/
				}
				/*dev*/	            else
					/*dev*/ {
					/*dev*/	                printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Ending firsTimeCheck if\n", tid, value);
					/*dev*/
				}
			}
			/*dev*/			    //END DEBUG


			firstTimeCheck++; //Provides that firstTimeCheck is more than 2.

			// p gets the first elemnt of openList
			// Note that it is the first value in previous iteration that is 
			// saved in openList

			openListAccess.lock();

#pragma omp critical
			{
				p = *openList.begin();
				i = p.second.first;
				j = p.second.second;
			}

#pragma omp critical
			{
				it = openList.begin();
			}


			/*dev*/			if (foundDest == 0)
				/*dev*/ {
				/*dev*/				//DEBUG
/*dev*/			    #ifdef PRINT_OPEN_LIST_BEFORE_PARALLEL
				/*dev*/				int for_iterator0 = 0;
				/*dev*/				sem_getvalue(&sem, &value);
				/*dev*/
				/*dev*/			    for (it = openList.begin(); it != openList.end(); it++)
					/*dev*/ {
					/*dev*/					if (tid == 0)
						/*dev*/ {
						/*dev*/					printf("SECTION = none THREAD = %d VALUE = %d : Before erasing element in openList Element [%d] = (%d,%d)\n", tid, value, for_iterator0, (*it).second.first, (*it).second.second);
						/*dev*/
					}
					/*dev*/					else
						/*dev*/ {
						/*dev*/						printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Before erasing element in openList Element [%d] = (%d,%d)\n", tid, value, for_iterator0, (*it).second.first, (*it).second.second);
						/*dev*/
					}
					/*dev*/					for_iterator0++;
					/*dev*/
				}
				/*dev*/				for_iterator0 = 0;
/*dev*/			    #endif // PRINT_OPEN_LIST_BEFORE_PARALLEL 
				/*dev*/
				/*dev*/				//END DEBUG  
				/*dev*/
			}

			// Remove this vertex from the open list 
			//5/31/2020
#pragma omp critical
			{
				openList.erase(openList.begin());
			}

			/*dev*/			if (foundDest == 0)
				/*dev*/ {
				/*dev*/				sem_getvalue(&sem, &value);
				/*dev*/				//DEBUG
/*dev*/			    #ifdef PRINT_OPEN_LIST_BEFORE_PARALLEL
/*dev*/
				/*dev*/			    for (it = openList.begin(); it != openList.end(); it++)
					/*dev*/ {
					/*dev*/					if (tid == 0)
						/*dev*/ {
						/*dev*/					printf("SECTION = none THREAD = %d VALUE = %d : After erasing element in openList Element [%d] = (%d,%d)\n", tid, value, *it, (*it).second.first, (*it).second.second);
						/*dev*/
					}
					/*dev*/					else
						/*dev*/ {
						/*dev*/						printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : After erasing element in openList Element [%d] = (%d,%d)\n", tid, value, *it, (*it).second.first, (*it).second.second);
						/*dev*/
					}
					/*dev*/
				}
				/*dev*/
/*dev*/			    #endif // PRINT_OPEN_LIST_BEFORE_PARALLEL 
/*dev*/
/*dev*/				//END DEBUG
/*dev*/
			}

			/*dev*/         if (tid == 0)
				/*dev*/ {
				/*dev*/		    printf("SECTION = none THREAD = %d VALUE = %d : Current cell is (%d,%d) (p.second.first,p.second.second) = \n", tid, value, p.second.first, p.second.second);
				/*dev*/
			}
			/*dev*/		    else
				/*dev*/ {
				/*dev*/			    printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Current cell is (%d,%d) (p.second.first,p.second.second) = \n", tid, value, p.second.first, p.second.second);
				/*dev*/
			}

			openListAccess.unlock();

			/*dev*/			if (tid == 0)
				/*dev*/ {
				/*dev*/		    printf("SECTION = none THREAD = %d VALUE = %d : Current cell is (%d,%d) (p.second.first,p.second.second) = \n", tid, value, p.second.first, p.second.second);
				/*dev*/
			}
			/*dev*/		    else
				/*dev*/ {
				/*dev*/			    printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Current cell is (%d,%d) (p.second.first,p.second.second) = \n", tid, value, p.second.first, p.second.second);
				/*dev*/
			}


			// Add this vertex to the closed list 
			/*#pragma omp critical
			{
				i = p.second.first;
				j = p.second.second;
			}*/

			/*dev*/			if (foundDest == 0)
				/*dev*/ {
				/*dev*/				//DEBUG
/*dev*/				#ifdef PRINT_CURRENT_CELL
/*dev*/
				/*dev*/				if (tid == 0)
					/*dev*/ {
					/*dev*/					printf("SECTION = none THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j, cellDetails[i][j].f);
					/*dev*/
				}
				/*dev*/				else
					/*dev*/ {
					/*dev*/					printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j, cellDetails[i][j].f);
					/*dev*/
				}
				/*dev*/
				/*dev*/				if (tid == 0)
					/*dev*/ {
					/*dev*/						printf("SECTION = none THREAD = %d VALUE = %d : Current cell is (%d,%d)\n", tid, value, i, j);
					/*dev*/
				}
				/*dev*/					else
					/*dev*/ {
					/*dev*/						printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d :  Current cell is (%d,%d)\n", tid, value, i, j);
					/*dev*/
				}
/*dev*/				#endif // PRINT_CURRENT_CELL
				/*dev*/				//END DEBUG
				/*dev*/
			}

			/*dev*/			if (foundDest == 0)
				/*dev*/ {
/*dev*/				#ifdef PRINT_CLOSED_LIST_BEFORE_PARALLEL
				/*dev*/				for (int i = 0; i < DEBUG_PRBL_WIDE; i++)
					/*dev*/ {
					/*dev*/					for (int j = 0; j < DEBUG_PRBL_WIDE; j++)
						/*dev*/ {
						/*dev*/						if (closedList[i][j] == true)
							/*dev*/ {
							/*dev*/							//DEBUG
/*dev*/							#ifdef PRINT_CURRENT_CELL
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/									printf("SECTION = none THREAD = %d VALUE = %d : Before adding an element to  closeList Element [%d][%d] = %d\n", tid, value, i, j, closedList[i][j]);
								/*dev*/
							}
							/*dev*/								else
								/*dev*/ {
								/*dev*/									printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : Before adding an element to  closeList Element [%d][%d] = %d\n", tid, value, i, j, closedList[i][j]);
								/*dev*/
							}
/*dev*/							#endif // PRINT_CURRENT_CELL
							/*dev*/							//END DEBUG
							/*dev*/
						}
						/*dev*/
						/*dev*/
					}
					/*dev*/
				}
/*dev*/				#endif // PRINT_CLOSED_LIST_BEFORE_PARALLEL
				/*dev*/
			}

#pragma omp critical
			{
				closedList[i][j] = true;
			}

			/*dev*/			if (foundDest == 0)
				/*dev*/ {
/*dev*/				#ifdef PRINT_CLOSED_LIST_BEFORE_PARALLEL
				/*dev*/					for (int i = 0; i < DEBUG_PRBL_WIDE; i++)
					/*dev*/ {
					/*dev*/						for (int j = 0; j < DEBUG_PRBL_WIDE; j++)
						/*dev*/ {
						/*dev*/							if (closedList[i][j] == true)
							/*dev*/ {
							/*dev*/								//DEBUG
/*dev*/							#ifdef PRINT_CURRENT_CELL
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/									printf("SECTION = none THREAD = %d VALUE = %d : After adding an element to  closeList Element [%d][%d] = %d\n", tid, value, i, j, closedList[i][j]);
								/*dev*/
							}
							/*dev*/								else
								/*dev*/ {
								/*dev*/									printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = none THREAD = %d VALUE = %d : After adding an element to  closeList Element [%d][%d] = %d\n", tid, value, i, j, closedList[i][j]);
								/*dev*/
							}
/*dev*/							#endif // PRINT_CURRENT_CELL
							/*dev*/							//END DEBUG
							/*dev*/
						}
						/*dev*/
					}
					/*dev*/
				}
/*dev*/				#endif // PRINT_CLOSED_LIST_BEFORE_PARALLEL		
				/*dev*/
			}
			/*
				Generating all the 4 successor of this cell

						N
						|
					W--Cell--E
						|
						S
				Cell-->Popped Cell (i, j)
				N --> North	 (i-1, j)
				S --> South	 (i+1, j)
				E --> East	 (i, j+1)
				W --> West   (i, j-1) */


			nthreads = omp_get_num_threads();
			tid = omp_get_thread_num();

			//***   North   ***//
			//Mesure North section time
			//-----------------------------------
			double startNorth;
			double endNorth;
			startNorth = omp_get_wtime();
			//------------------------------------

			if (foundDest == false)
			{
				int check_counter = 0;
				while (check_counter < LOAD)
				{
					check_counter++;
				}
				check_counter = 0;

				tid = omp_get_thread_num();
/*dev*/				#ifdef DEBUG_MESSAGES_SEVERITY_ALL
				/*dev*/					if (tid == 0)
					/*dev*/ {
					/*dev*/						printf("\n\n\n SECTION = Section 1 (North) THREAD = %d VALUE = %d : -North = (%d.%d)\n", tid, value, i - 1, j);
					/*dev*/
				}
				/*dev*/					else
					/*dev*/ {
					/*dev*/						printf("\n\n\n \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : -North = (%d.%d)\n", tid, value, i - 1, j);
					/*dev*/
				}
/*dev*/				#endif // DEBUG_MESSAGES_SEVERITY_ALL

				//----------- 1st Successor (North) ------------ 
				//i = ROW
				//j = COL


				//CellAcess.lock();

				//printf("SECTION = Section 1 (North) THREAD = %d VALUE = %d : CellAcess locked\n",tid,value);

				// Only process this cell if this is a valid one 
				if (isValid(i - 1, j) == true)
				{
					// If the destination cell is the same as the 
					// current successor 
					if (isDestination(i - 1, j, dest) == true)
					{
						// Set the Parent of the destination cell 
						cellDetails[i - 1][j].parent_i = i;
						cellDetails[i - 1][j].parent_j = j;

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf("SECTION = Section 1 (North) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						};
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL

						tracePath(cellDetails, output_file_map, dest, src);

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 1 (North) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL
						foundDest = true;
						//return;
					}

					// If the successor is already on the closed 
					// list or if it is blocked, then ignore it. 
					// Else do the following 
					else if (closedList[i - 1][j] == false &&
						isUnBlocked(grid, i - 1, j) == true)
					{
						payloadTransition.lock();
#pragma omp critical
						{
							fNew = cellDetails[i][j].f + 1.0;
						}
						//payloadTransition.unlock();


/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/
						/*dev*/                        if (tid == 0)
							/*dev*/ {
							/*dev*/								printf("SECTION = Section 1 (North) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i - 1, j, cellDetails[i - 1][j].f);
							/*dev*/
						}
						/*dev*/							else
							/*dev*/ {
							/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i - 1, j, cellDetails[i - 1][j].f);
							/*dev*/
						}
						/*dev*/
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 1 (North) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
/*dev*/						#endif //DEBUG_MESSAGES_SEVERITY_ALL


						// If it isn’t on the open list, add it to 
						// the open list. Make the current square 
						// the parent of this square. Record the 
						// f, g, and h costs of the square cell 
						//			 OR 
						// If it is on the open list already, check 
						// to see if this path to that square is better, 
						// using 'f' cost as the measure. 
						if (cellDetails[i - 1][j].f == FLT_MAX ||
							cellDetails[i - 1][j].f > fNew)
						{

							openListAccess.lock();
#pragma omp critical
							{
								openList.insert(make_pair(fNew, make_pair(i - 1, j)));
							}

							sem_getvalue(&sem, &value);

							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf(" SECTION = Section 1 (North) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i - 1, j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i - 1, j);
								/*dev*/
							}
							sem_post(&sem);

							/*dev*/							sem_getvalue(&sem, &value);
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf(" SECTION = Section 1 (North) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i - 1, j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i - 1, j);
								/*dev*/
							}
							openListAccess.unlock();
/*dev*/							#ifdef DEBUG_MESSAGES_SEVERITY_ALL
							/*dev*/								int for_iterator1 = 0;
							/*dev*/								for (it = openList.begin(); it != openList.end(); it++)
								/*dev*/ {
								/*dev*/									if (tid == 0)
									/*dev*/ {
									/*dev*/									printf("SECTION = Section 1 (North) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator1, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/									else
									/*dev*/ {
									/*dev*/										printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator1, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/									for_iterator1++;
								/*dev*/
							}
							/*dev*/								for_iterator1 = 0;
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL


							// Update the details of this cell
							//payloadTransition.lock();
#pragma omp critical
							{
								cellDetails[i - 1][j].f = fNew;
								cellDetails[i - 1][j].parent_i = i;
								cellDetails[i - 1][j].parent_j = j;
							}
							//payloadTransition.unlock();

/*dev*/							#ifdef DEBUG_MESSAGES_SEVERITY_ALL
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf("SECTION = Section 1 (North) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i - 1, j, cellDetails[i - 1][j].f);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i - 1, j, cellDetails[i - 1][j].f);
								/*dev*/
							}
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf("SECTION = Section 1 (North) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i - 1, j, cellDetails[i - 1][j].parent_i, cellDetails[i - 1][j].parent_j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 1 (North) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i - 1, j, cellDetails[i - 1][j].parent_i, cellDetails[i - 1][j].parent_j);
								/*dev*/
							}
							/*dev*/
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL

						} //end of if (cellDetails[i - 1][j].f == FLT_MAX || cellDetails[i - 1][j].f > fNew
						payloadTransition.unlock();
					}//end of else if (closedList[i - 1][j] == false && isUnBlocked(grid, i - 1, j) == true)
				}// end of isValid
				//CellAcess.unlock();
			} //end of if (foundDest == false)

			/*dev*/			printf("\n\n\nSECTION = Section 1 (North) THREAD = %d VALUE = %d : CellAcess unlocked\n", tid, value);
			/*dev*/			endNorth = omp_get_wtime();
/*dev*/			#ifdef PRINT_NORTH_SECTION_PERFORMING_TIME
			/*dev*/				printf("~Work took North Section %f seconds\n~", (endNorth - startNorth));
/*dev*/			#endif // PRINT_NORTH_SECTION_PERFORMING_TIME

			//***   South   ***//
			//Mesure South section time
			//-----------------------------------
			double startSouth;
			double endSouth;
			startSouth = omp_get_wtime();
			//------------------------------------

			if (foundDest == false)
			{

				int check_counter = 0;
				while (check_counter < LOAD)
				{
					check_counter++;
				}
				check_counter = 0;
				tid = omp_get_thread_num();
/*dev*/				#ifdef DEBUG_MESSAGES_SEVERITY_ALL
				/*dev*/					if (tid == 0)
					/*dev*/ {
					/*dev*/						printf("\n\n\n SECTION = Section 2 (South) THREAD = %d VALUE = %d : -South = (%d.%d)\n", tid, value, i + 1, j);
					/*dev*/
				}
				/*dev*/					else
					/*dev*/ {
					/*dev*/						printf("\n\n\n \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : -South = (%d.%d)\n", tid, value, i + 1, j);
					/*dev*/
				}
/*dev*/				#endif // DEBUG_MESSAGES_SEVERITY_ALL


				//----------- 2nd Successor (South) ------------ 
				// Only process this cell if this is a valid one 
				//CellAcess.lock();

				//printf("SECTION = Section 2 (South) THREAD = %d VALUE = %d : CellAcess locked\n",tid,value);
				if (isValid(i + 1, j) == true)
				{
					// If the destination cell is the same as the 
					// current successor 
					if (isDestination(i + 1, j, dest) == true)
					{
						// Set the Parent of the destination cell 
						cellDetails[i + 1][j].parent_i = i;
						cellDetails[i + 1][j].parent_j = j;

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf("SECTION = Section 2 (South) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						};
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL


						tracePath(cellDetails, output_file_map, dest, src);

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 2 (South) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL
						foundDest = true;
						//return;
					}
					// If the successor is already on the closed 
					// list or if it is blocked, then ignore it. 
					// Else do the following 

					else if (closedList[i + 1][j] == false &&
						isUnBlocked(grid, i + 1, j) == true)
					{
						//printf("closedList[%d + 1][%d] = %d\n",i,j,closedList[i + 1][j]);
						//printf("isUnBlocked(grid, %d + 1, %d) == %d\n",i,j,isUnBlocked(grid, i + 1, j));

						payloadTransition.lock();
#pragma omp critical
						{
							fNew = cellDetails[i][j].f + 1.0;
						}
						//payloadTransition.unlock();

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/
						/*dev*/                        if (tid == 0)
							/*dev*/ {
							/*dev*/								printf("SECTION = Section 2 (South) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i + 1, j, cellDetails[i + 1][j].f);
							/*dev*/
						}
						/*dev*/							else
							/*dev*/ {
							/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i + 1, j, cellDetails[i + 1][j].f);
							/*dev*/
						}
						/*dev*/
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 2 (South) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = 2 (South) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
/*dev*/						#endif //DEBUG_MESSAGES_SEVERITY_ALL


						// If it isn’t on the open list, add it to 
						// the open list. Make the current square 
						// the parent of this square. Record the 
						// f, g, and h costs of the square cell 
						//			 OR 
						// If it is on the open list already, check 
						// to see if this path to that square is better, 
						// using 'f' cost as the measure. 
						if (cellDetails[i + 1][j].f == FLT_MAX ||
							cellDetails[i + 1][j].f > fNew)
						{
							openListAccess.lock();
#pragma omp critical
							{
								openList.insert(make_pair(fNew, make_pair(i + 1, j)));
							}


							/*dev*/							sem_getvalue(&sem, &value);
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf(" SECTION = Section 2 (South) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i + 1, j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i + 1, j);
								/*dev*/
							}
							sem_post(&sem);

							/*dev*/							sem_getvalue(&sem, &value);
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf(" SECTION = Section 2 (South) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i + 1, j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i + 1, j);
								/*dev*/
							}
							/*dev*/
							/*dev*/							openListAccess.unlock();
/*dev*/							#ifdef DEBUG_MESSAGES_SEVERITY_ALL
							/*dev*/								int for_iterator2 = 0;
							/*dev*/								for (it = openList.begin(); it != openList.end(); it++)
								/*dev*/ {
								/*dev*/									if (tid == 0)
									/*dev*/ {
									/*dev*/									printf("SECTION = Section 2 (South) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator2, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/									else
									/*dev*/ {
									/*dev*/										printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator2, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/									for_iterator2++;
								/*dev*/
							}
							/*dev*/								for_iterator2 = 0;
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL


							// Update the details of this cell
							//payloadTransition.lock();
#pragma omp critical
							{
								cellDetails[i + 1][j].f = fNew;
								cellDetails[i + 1][j].parent_i = i;
								cellDetails[i + 1][j].parent_j = j;
							}
							//payloadTransition.unlock();

#ifdef DEBUG_MESSAGES_SEVERITY_ALL

							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf("SECTION = Section 2 (South) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i + 1, j, cellDetails[i + 1][j].f);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i + 1, j, cellDetails[i + 1][j].f);
								/*dev*/
							}
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf("SECTION = Section 2 (South) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i + 1, j, cellDetails[i + 1][j].parent_i, cellDetails[i + 1][j].parent_j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 2 (South) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i + 1, j, cellDetails[i + 1][j].parent_i, cellDetails[i + 1][j].parent_j);
								/*dev*/
							}
							/*dev*/
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL
						}
						payloadTransition.unlock();
					}
				}
				//CellAcess.unlock();
			}

			/*dev*/			printf("\n\n\nSECTION = Section 2 (South) THREAD = %d VALUE = %d : CellAcess unlocked\n", tid, value);
			/*dev*/			endSouth = omp_get_wtime();
/*dev*/			#ifdef PRINT_SOUTH_SECTION_PERFORMING_TIME
			/*dev*/				printf("~Work took South Section %f seconds\n~", (endSouth - startSouth));
/*dev*/			#endif // PRINT_SOUTH_SECTION_PERFORMING_TIME

			//***   EAST   ***//	
			//Mesure East section time
			//-----------------------------------
			double startEast;
			double endEast;
			startEast = omp_get_wtime();
			//------------------------------------

			if (foundDest == false)
			{
				tid = omp_get_thread_num();
				int check_counter = 0;
				while (check_counter < LOAD)
				{
					check_counter++;
				}
				check_counter = 0;

/*dev*/				#ifdef DEBUG_MESSAGES_SEVERITY_ALL
				/*dev*/					if (tid == 0)
					/*dev*/ {
					/*dev*/						printf("\n\n\n SECTION = Section 3 (East) THREAD = %d VALUE = %d : -East = (%d.%d)\n", tid, value, i, j + 1);
					/*dev*/
				}
				/*dev*/					else
					/*dev*/ {
					/*dev*/						printf("\n\n\n \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : -East = (%d.%d)\n", tid, value, i, j + 1);
					/*dev*/
				}
/*dev*/				#endif // DEBUG_MESSAGES_SEVERITY_ALL


				//----------- 3rd Successor (East) ------------ 
				//CellAcess.lock();
				//printf("SECTION = Section 3 (East) THREAD = %d VALUE = %d : CellAcess locked\n",tid,value);
				// Only process this cell if this is a valid one 
				if (isValid(i, j + 1) == true)
				{
					// If the destination cell is the same as the 
					// current successor 
					if (isDestination(i, j + 1, dest) == true)
					{

						// Set the Parent of the destination cell
						cellDetails[i][j + 1].parent_i = i;
						cellDetails[i][j + 1].parent_j = j;


/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf("SECTION = Section 3 (East) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						};
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL

						tracePath(cellDetails, output_file_map, dest, src);

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 3 (East) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL
						foundDest = true;
						//return;
					}

					// If the successor is already on the closed 
					// list or if it is blocked, then ignore it. 
					// Else do the following 
					else if (closedList[i][j + 1] == false &&
						isUnBlocked(grid, i, j + 1) == true)
					{
						payloadTransition.lock();
#pragma omp critical
						{
							fNew = cellDetails[i][j].f + 1.0;
						}
						//payloadTransition.unlock();


#ifdef DEBUG_MESSAGES_SEVERITY_ALL

						/*dev*/                        if (tid == 0)
							/*dev*/ {
							/*dev*/								printf("SECTION = Section 3 (East) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j + 1, cellDetails[i][j + 1].f);
							/*dev*/
						}
						/*dev*/							else
							/*dev*/ {
							/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j + 1, cellDetails[i][j + 1].f);
							/*dev*/
						}
						/*dev*/
						/*dev*/
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 3 (East) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
/*dev*/						#endif //DEBUG_MESSAGES_SEVERITY_ALL


						// If it isn’t on the open list, add it to 
						// the open list. Make the current square 
						// the parent of this square. Record the 
						// f, g, and h costs of the square cell 
						//			 OR 
						// If it is on the open list already, check 
						// to see if this path to that square is better, 
						// using 'f' cost as the measure. 
						if (cellDetails[i][j + 1].f == FLT_MAX ||
							cellDetails[i][j + 1].f > fNew)
						{
							openListAccess.lock();

#pragma omp critical
							{
								openList.insert(make_pair(fNew, make_pair(i, j + 1)));
							}





							/*dev*/							sem_getvalue(&sem, &value);

							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf(" SECTION = Section 3 (East) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i, j + 1);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i, j + 1);
								/*dev*/
							}
							sem_post(&sem);

							/*dev*/							sem_getvalue(&sem, &value);
							/*dev*/
							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf(" SECTION = Section 3 (East) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i, j + 1);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i, j + 1);
								/*dev*/
							}
							openListAccess.unlock();
/*dev*/							#ifdef DEBUG_MESSAGES_SEVERITY_ALL
							/*dev*/								int for_iterator3 = 0;
							/*dev*/								for (it = openList.begin(); it != openList.end(); it++)
								/*dev*/ {
								/*dev*/									if (tid == 0)
									/*dev*/ {
									/*dev*/									printf("SECTION = Section 3 (East) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator3, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/									else
									/*dev*/ {
									/*dev*/										printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator3, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/									for_iterator3++;
								/*dev*/
							}
							/*dev*/								for_iterator3 = 0;
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL


							// Update the details of this cell
							//payloadTransition.lock();
#pragma omp critical
							{
								cellDetails[i][j + 1].f = fNew;
								cellDetails[i][j + 1].parent_i = i;
								cellDetails[i][j + 1].parent_j = j;
							}
							//payloadTransition.unlock();

#ifdef DEBUG_MESSAGES_SEVERITY_ALL

							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf("SECTION = Section 3 (East) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j + 1, cellDetails[i][j + 1].f);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j + 1, cellDetails[i][j + 1].f);
								/*dev*/
							}

							/*dev*/							if (tid == 0)
								/*dev*/ {
								/*dev*/								printf("SECTION = Section 3 (East) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i, j + 1, cellDetails[i][j + 1].parent_i, cellDetails[i][j + 1].parent_j);
								/*dev*/
							}
							/*dev*/							else
								/*dev*/ {
								/*dev*/								printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 3 (East) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i, j + 1, cellDetails[i][j + 1].parent_i, cellDetails[i][j + 1].parent_j);
							}
							/*dev*/
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL
						}
						payloadTransition.unlock();
					}
				}
				//CellAcess.unlock();
			}

			/*dev*/				printf("\n\n\nSECTION = Section 3 (East) THREAD = %d VALUE = %d : CellAcess unlocked\n", tid, value);
			// end of foundtest flag checking
			//endEast = omp_get_wtime();	
/*dev*/			#ifdef PRINT_EAST_SECTION_PERFORMING_TIME
			/*dev*/				printf("~Work took East Section %f seconds\n~", (endEast - startEast));
/*dev*/			#endif // PRINT_EAST_SECTION_PERFORMING_TIME

			//***   West   ***//	
			//Mesure West section time
			//-----------------------------------
			double startWest;
			double endWest;
			//startWest = omp_get_wtime();
			//------------------------------------

			if (foundDest == false)
			{
				//printf("here4\n");
				tid = omp_get_thread_num();
				int check_counter = 0;
				while (check_counter < LOAD)
				{
					check_counter++;
				}
				check_counter = 0;

/*dev*/				#ifdef DEBUG_MESSAGES_SEVERITY_ALL
				/*dev*/					if (tid == 0)
					/*dev*/ {
					/*dev*/						printf("\n\n\n SECTION = Section 4 (West) THREAD = %d VALUE = %d : -West = (%d.%d)\n", tid, value, i, j - 1);
					/*dev*/
				}
				/*dev*/					else
					/*dev*/ {
					/*dev*/						printf("\n\n\n \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : -West = (%d.%d)\n", tid, value, i, j - 1);
					/*dev*/
				}
/*dev*/				#endif // DEBUG_MESSAGES_SEVERITY_ALL
				//----------- 4th Successor (West) ------------ 
				//CellAcess.lock();
				/*dev*/				printf("SECTION = Section 4 (West) THREAD = %d VALUE = %d : CellAcess locked\n", tid, value);
				// Only process this cell if this is a valid one 
				if (isValid(i, j - 1) == true)
				{
					// If the destination cell is the same as the 
					// current successor 
					if (isDestination(i, j - 1, dest) == true)
					{
						// Set the Parent of the destination cell

						cellDetails[i][j - 1].parent_i = i;
						cellDetails[i][j - 1].parent_j = j;

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf("SECTION = Section 4 (West) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : The destination cell has found\n", tid, value);
							/*dev*/
						}
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL

						tracePath(cellDetails, output_file_map, dest, src);

/*dev*/						#ifdef DEBUG_MESSAGES_SEVERITY_ALL
						/*dev*/						if (tid == 0)
							/*dev*/ {
							/*dev*/							printf(" SECTION = Section 4 (West) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
						/*dev*/						else
							/*dev*/ {
							/*dev*/							printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : Exit trace path\n", tid, value);
							/*dev*/
						}
/*dev*/						#endif // DEBUG_MESSAGES_SEVERITY_ALL
						foundDest = true;
						//return;
					}

					// If the successor is already on the closed 
					// list or if it is blocked, then ignore it. 
					// Else do the following 
					else if (closedList[i][j - 1] == false &&
						isUnBlocked(grid, i, j - 1) == true)
					{
						payloadTransition.lock();
#pragma omp critical
						{
							fNew = cellDetails[i][j].f + 1.0;
						}
						//payloadTransition.unlock();

/*dev*/							#ifdef DEBUG_MESSAGES_SEVERITY_ALL
/*dev*/
						/*dev*/                            if (tid == 0)
							/*dev*/ {
							/*dev*/										printf("SECTION = Section 4 (West) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j - 1, cellDetails[i][j - 1].f);
							/*dev*/
						}
						/*dev*/									else
							/*dev*/ {
							/*dev*/										printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j - 1, cellDetails[i][j - 1].f);
							/*dev*/
						}
						/*dev*/
						/*dev*/							if (tid == 0)
							/*dev*/ {
							/*dev*/								printf(" SECTION = Section 4 (West) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
						/*dev*/							else
							/*dev*/ {
							/*dev*/								printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : fNew = %lf\n", tid, value, fNew);
							/*dev*/
						}
/*dev*/							#endif //DEBUG_MESSAGES_SEVERITY_ALL

						// If it isn’t on the open list, add it to 
						// the open list. Make the current square 
						// the parent of this square. Record the 
						// f, g, and h costs of the square cell 
						//			 OR 
						// If it is on the open list already, check 
						// to see if this path to that square is better, 
						// using 'f' cost as the measure. 
						if (cellDetails[i][j - 1].f == FLT_MAX ||
							cellDetails[i][j - 1].f > fNew)
						{
							openListAccess.lock();

#pragma omp critical
							{
								openList.insert(make_pair(fNew, make_pair(i, j - 1)));
							}




							/*dev*/								sem_getvalue(&sem, &value);
							/*dev*/
							/*dev*/								if (tid == 0)
								/*dev*/ {
								/*dev*/									printf(" SECTION = Section 4 (West) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i, j - 1);
								/*dev*/
							}
							/*dev*/								else
								/*dev*/ {
								/*dev*/									printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted Before semaphore unlocking\n", tid, value, i, j - 1);
								/*dev*/
							}
							sem_post(&sem);

							/*dev*/								sem_getvalue(&sem, &value);

							/*dev*/								if (tid == 0)
								/*dev*/ {
								/*dev*/									printf(" SECTION = Section 4 (West) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i, j - 1);
								/*dev*/
							}
							/*dev*/								else
								/*dev*/ {
								/*dev*/									printf(" \t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : Pair (%d,%d) has been inserted After semaphore unlocking\n", tid, value, i, j - 1);
								/*dev*/
							}
							openListAccess.unlock();
/*dev*/								#ifdef DEBUG_MESSAGES_SEVERITY_ALL
							/*dev*/									printf("Section 4 (West):THREAD %d Elements in openList\n", tid);
							/*dev*/									it = openList.begin();
							/*dev*/									int for_iterator4 = 0;
							/*dev*/									for (it = openList.begin(); it != openList.end(); it++)
								/*dev*/ {
								/*dev*/										if (tid == 0)
									/*dev*/ {
									/*dev*/											printf("SECTION = Section 4 (West) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator4, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/										else
									/*dev*/ {
									/*dev*/											printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : openList Element [%d] = (%d,%d)\n", tid, value, for_iterator4, (*it).second.first, (*it).second.second);
									/*dev*/
								}
								/*dev*/										for_iterator4++;
								/*dev*/
							}
							/*dev*/									for_iterator4 = 0;
/*dev*/								#endif //DEBUG_MESSAGES_SEVERITY_ALL

							// Update the details of this cell

							//payloadTransition.lock(); 
#pragma omp critical
							{
								cellDetails[i][j - 1].f = fNew;
								cellDetails[i][j - 1].parent_i = i;
								cellDetails[i][j - 1].parent_j = j;
							}
							//payloadTransition.lock();


/*dev*/								#ifdef DEBUG_MESSAGES_SEVERITY_ALL
/*dev*/
							/*dev*/									if (tid == 0)
								/*dev*/ {
								/*dev*/										printf("SECTION = Section 4 (West) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j - 1, cellDetails[i][j - 1].f);
								/*dev*/
							}
							/*dev*/									else
								/*dev*/ {
								/*dev*/										printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : cellDetails[%d][%d].f = %lf \n", tid, value, i, j - 1, cellDetails[i][j - 1].f);
								/*dev*/
							}

							/*dev*/									if (tid == 0)
								/*dev*/ {
								/*dev*/										printf("SECTION = Section 4 (West) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i, j - 1, cellDetails[i][j - 1].parent_i, cellDetails[i][j - 1].parent_j);
								/*dev*/
							}
							/*dev*/									else
								/*dev*/ {
								/*dev*/										printf("\t\t\t\t\t\t\t\t\t\t\t\t\t SECTION = Section 4 (West) THREAD = %d VALUE = %d : cellDetails[%d][%d].parent_i, parent_j => (%d,%d) = %lf \n", tid, value, i, j - 1, cellDetails[i][j - 1].parent_i, cellDetails[i][j - 1].parent_j);
								/*dev*/
							}
							/*dev*/
/*dev*/								#endif //DEBUG_MESSAGES_SEVERITY_ALL

						}// if (cellDetails[i][j - 1].f == FLT_MAX || cellDetails[i][j - 1].f > fNew)
						payloadTransition.unlock();
					}// else if (closedList[i][j - 1] == false && isUnBlocked(grid, i, j - 1) == true)
				}// if (isValid(i, j - 1) == true)
				//CellAcess.unlock();
				printf("\n\n\nSECTION = Section 4 (West) THREAD = %d VALUE = %d : CellAcess unlocked\n", tid, value);

				/*dev*/				endWest = omp_get_wtime();
/*dev*/				#ifdef PRINT_EAST_SECTION_PERFORMING_TIME
				/*dev*/					printf("~Work took West Section %f seconds\n~", (endWest - startWest));
/*dev*/				#endif // PRINT_EAST_SECTION_PERFORMING_TIME
			}// if(tid==thread_arrangment.fourth_section)

			/*dev*/		printf("\n\n\nSECTION = none THREAD = %d VALUE = %d : After sides\n", tid, value);
			//}// foundDest = 0	
		}//End of while
		/*dev*/		printf("\n\n\nSECTION = none THREAD = %d VALUE = %d : End of while\n", tid, value);
	}// pragma omp block of code
	/*dev*/	printf("\n\n\nSECTION = none Exit of parallel section \n");

	// When the destination cell is not found and the open 
	// list is empty, then we conclude that we failed to 
	// reach the destiantion cell. This may happen when the 
	// there is no way to destination cell (due to blockages) 
	if (foundDest == false) {
#ifdef DEBUG_MESSAGES_SEVERITY_ALL
		printf("Failed to find the Destination Cell\n");
#endif // DEBUG_MESSAGES_SEVERITY_ALL
	}
	else {

#ifdef DEBUG_MESSAGES_SEVERITY_ALL
		printf("Return \n");
#endif // DEBUG_MESSAGES_SEVERITY_PRESULT

		endaStarSearch = omp_get_wtime();

#ifdef PRINT_ASTARTSEARCH_TIME_PERFORMING
		printf("*********************************************************************************\n");
		printf("~Work took aStarSearch Section %f seconds\n~", (endaStarSearch - startaStarSearch));
		printf("*********************************************************************************\n");
#endif // PRINT_ASTARTSEARCH_TIME_PERFORMING

		return;
	}

	printf("The last line of the function \n");
}

int MatrixInfo_txt(string path_name, string& map)
{
	ifstream input(path_name);
	for (int i = 0; !input.eof(); i++)
	{
		map.resize(i + 1);
		input >> map[i];
	}

	return map.size() - 1;
}

void getInputSize(int* col_row, int matrix_size)
{
	int done = 0;

	for (int i = 1; !done; i++)
	{
		//printf("i = %d\n",i);
		*col_row = matrix_size / i;
		if (*col_row == i) {
			done = 1;
		}
	}

	//Static debug messages
	//printf("matrix size is %d\n",matrix_size);
	//printf("col_row is %d\n",*col_row);
}

// Driver program to test above function 
int main(int argc, char* argv[])
{

	double start;
	double end;
	start = omp_get_wtime();

#ifdef CMD_Linux
	if (argc == 3) {
		printf("The program called succesfully\n");
	}
	else if (argc > 3) {
		printf("Too many arguments supplied.\n");
		printf("Please, call binary like\n");
		printf("./Astar example.txt 4\n");
		exit(1);
	}
	else {
		printf("Too few arguments supplied.\n");
		printf("Please, call binary like\n");
		printf("./Astar example.txt\n");
		exit(2);
	}

#endif // CMD_Linux

#ifdef CMD_Linux
	string path_name = argv[1];
	int threads = (int)(*argv[2]) - 48;
#endif //CMD_Linux

#ifdef IDE_USING
	string path_name = DEBUG_PATH_MTRX;
#endif

	string map = "";
	int matrix_size = MatrixInfo_txt(path_name, map);

	//printf("here\n"); 
	//Get col/row from txt
	getInputSize(&col_row, matrix_size);
	//printf("here1\n");
	//vector<vector<int>> grid(col_row, vector<int>(col_row, 0));

	int  src_dest_row[2]; // [1] source row [2] destination row
	int  src_dest_col[2]; // [1] source col [2] destination col

	vector<vector<int>> grid(col_row, vector<int>(col_row, 0));

	//Old version of creating vector of vector. It does not work with bigger col_row
	//vector<vector<cell> > cellDetails(col_row*col_row, vector<cell>(col_row*col_row));

	/*
	Set fields for each of the fields
	f = g = Maximum type value
	parent_i = parent_j = -1 (default value)
	*/
	//int i, j;
/*
	for ( i = 0; i < col_row; i++) {
		// Vector to store column elements
		vector<int> column;

		for ( j = 0; j < col_row; j++) {
			column.push_back(0);
		}

		// Pushing back above 1D vector
		// to create the 2D vector
		grid.push_back(column);
	} */


	for (int i = 0; i < col_row; i++)
	{
		for (int j = 0; j < col_row; j++)
		{
			if (map[i * col_row + j] == 'S') {
				grid[i][j] = (int)map[i * col_row + j] - 'S';
				src_dest_row[0] = i;
				src_dest_col[0] = j;
			}

			if (map[i * col_row + j] == 'E') {
				grid[i][j] = (int)map[i * col_row + j] - 'E';
				src_dest_row[1] = i;
				src_dest_col[1] = j;
			}
			if (map[i * col_row + j] == '0' || map[i * col_row + j] == '1')
				grid[i][j] = (int)map[i * col_row + j] - '0';
		}
	}

	//Static debug messages
	//printf("Source\n");
	printf("Source (%d,%d)\n", src_dest_col[0], src_dest_row[0]);

	//printf("Destination\n");
	printf("Destin (%d,%d)\n", src_dest_col[1], src_dest_row[1]);
	// Source 
	Pair src = make_pair(src_dest_col[0], src_dest_row[0]);

	// Destination
	Pair dest = make_pair(src_dest_col[1], src_dest_row[1]);

	aStarSearch(grid, src, dest, threads);

	end = omp_get_wtime();

#ifdef PRINT_THE_WHOLE_PROGRAM_PERFORMING_TIME
	printf("****************************************************************\n");
	printf("~Work took %f seconds\n~", (end - start));
	printf("****************************************************************\n");
#endif // PRINT_THE_WHOLE_PROGRAM_PERFORMING_TIME


	return(0);
}