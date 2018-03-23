#include<iostream>
#include<queue>
#define MAXNODE 50
using namespace std;

typedef struct BitNode {
	char data;
	struct BitNode * left;
	struct BitNode * right;
}BitTree;

typedef struct EdgeNode {
	int adjvex;
	struct EdgeNode *next_edge;
}EdgeNode;

typedef struct {
	char data;
	EdgeNode *first_edge;
}VertexNode;

typedef struct {
	VertexNode vertexes[MAXNODE];
	int node_num;
	int edge_num;
}Graph;

void swap(int&, int&);
void p(int[], int);
void insert_sort(int[], int);
void shell_sort(int[], int);
void bubble_sort(int[], int);
int partition(int[], int, int);
void QuickSort(int[], int, int);
void select_sort(int[], int);
void heap_sort(int[], int);
void BuildMaxHeap(int[], int);
void AdjustDown(int[], int, int);
void merge(int[], int, int, int);
void MergeSort(int[], int, int);
void create_tree(BitTree *&);
void preorder(BitTree *);
void inorder(BitTree*);
void postorder(BitTree*);
void CreateGraph(Graph * &);
void DisplayGraph(Graph *);
void DFS(Graph*, int);
void BFS(Graph*, int);
void DFSTraverse(Graph*);
void BFSTraverse(Graph*);

int main() {
	int a[] = { 3,4,1,6,15,8,2,11,9,10,7,5 };
	int length = sizeof(a) / sizeof(a[0]);
	heap_sort(a,length);
	p(a,length);
	/*BitTree *T;

	cout << "create tree:" << endl;
	creat_tree(T);
	cout << endl;

	cout << "preorder:" << endl;
	preorder(T);
	cout << endl;

	cout << "inorder:" << endl;
	inorder(T);
	cout << endl;

	cout << "postorder:" << endl;
	postorder(T);
	cout << endl;*/

	/*Graph *G;
	CreateGraph(G);
	cout << "-----display-----" << endl;
	DisplayGraph(G);
	cout << "-----DFS-----" << endl;
	DFSTraverse(G);
	cout << "-----BFS-----" << endl;
	BFSTraverse(G);*/

	system("pause");
	return 0;
}

void swap(int &a, int &b) {
	int temp = a;
	a = b;
	b = temp;
}

void p(int a[], int n) {
	for (int i = 0; i<n; i++)
		cout << a[i] << endl;
}

void insert_sort(int a[], int n) {
	int i, j;
	for (i = 1; i < n; i++) {
		if (a[i - 1] > a[i]) {
			int key = a[i];
			for (j = i - 1; j >= 0 && key < a[j]; j--) {
				a[j + 1] = a[j];
			}
			a[j + 1] = key;
		}
	}
}

void shell_sort(int a[], int n) {
	for (int step = n / 2; step >= 1; step /= 2) {
		for (int i = step + 1; i < n; i++) {
			if (a[i] < a[i - step]) {
				int temp = a[i];
				int j;
				for (j = i - step; j >= 0 && temp < a[j]; j -= step) {
					a[j + step] = a[j];
				}
				a[j + step] = temp;
			}
		}
	}
}

void bubble_sort(int a[], int n) {
	for (int i = 0; i < n - 1; i++) {
		bool flag = false;
		for (int j = n - 1; j > i; j--) {
			if (a[j] < a[j - 1]) {
				swap(a[j], a[j - 1]);
				flag = true;
			}
			if (flag == false) {
				break;
			}
		}
	}
}

int partition(int a[], int low, int high) {
	int key = a[low];
	int index = low;
	while (low < high) {
		while (a[high] > key)
			high--;
		swap(a[index], a[high]);
		index = high;
		while (a[low] < key)
			low++;
		swap(a[index], a[low]);
		index = low;
	}
	return index;
}

void QuickSort(int a[], int low, int high) {
	if (low < high) {
		int pos = partition(a, low, high);
		QuickSort(a, low, pos - 1);
		QuickSort(a, pos + 1, high);
	}
}

void select_sort(int a[], int n) {
	for (int i = 0; i < n; i++) {
		int min = a[i];
		int index = i;
		for (int j = i + 1; j < n; j++) {
			if (min > a[j]) {
				min = a[j];
				index = j;
			}
		}
		swap(a[i], a[index]);
	}
}

void heap_sort(int a[], int n) {
	BuildMaxHeap(a, n);
	for (int i = n - 1; i > 0; i--) {
		swap(a[0], a[i]);
		AdjustDown(a, 0, i - 1);
	}
}

void BuildMaxHeap(int a[], int n) {
	for (int i = n / 2; i >= 0; i--) {
		AdjustDown(a, i, n);
	}
}

void AdjustDown(int a[], int k, int n) {
	for (int i = 2 * k; i < n; i *= 2) {
		if (a[i] < a[i + 1]) {
			i++;
		}
		if (a[k] >= a[i]) {
			break;
		}
		else {
			swap(a[i], a[k]);
			k = i;
		}
	}
}

int *b = (int*)malloc(10 * sizeof(int));

void merge(int a[], int low, int mid, int high) {
	int i, j, k;
	for (i = low; i <= high; i++) {
		b[i] = a[i];
	}
	for (i = low, j = mid + 1, k = i; i <= mid &&j <= high; k++) {
		if (b[i] <= b[j])
			a[k] = b[i++];
		else
			a[k] = b[j++];
	}
	while (i <= mid)
		a[k++] = b[i++];
	while (j <= high)
		a[k++] = b[j++];
}

void MergeSort(int a[], int low, int high) {
	if (low < high) {
		int mid = (low + high) / 2;
		MergeSort(a, low, mid);
		MergeSort(a, mid + 1, high);
		merge(a, low, mid, high);
	}
}

void creat_tree(BitTree * &T) {
	char data;
	cin >> data;
	if (data == '#')
		T = NULL;
	else {
		T = new BitNode;
		T->data = data;

		creat_tree(T->left);
		creat_tree(T->right);
	}
}

void preorder(BitTree *T) {
	if (T == NULL)
		return;
	else {
		cout << T->data;
		preorder(T->left);
		preorder(T->right);
	}
}

void inorder(BitTree *T) {
	if (T == NULL)
		return;
	else {
		preorder(T->left);
		cout << T->data;
		preorder(T->right);
	}
}

void postorder(BitTree *T) {
	if (T == NULL)
		return;
	else {
		preorder(T->left);
		preorder(T->right);
		cout << T->data;
	}
}

void CreateGraph(Graph * &G) {
	G = (Graph*)malloc(sizeof(Graph));

	cout << "输入顶点数:";
	cin >> G->node_num;
	cout << "输入边数:";
	cin >> G->edge_num;

	for (int i = 0; i < G->node_num; i++) {
		cout << "输入顶点 " << i  << "信息：";
		cin >> G->vertexes[i].data;
		G->vertexes[i].first_edge = NULL;
	}

	for (int k = 0; k < G->edge_num; k++) {
		cout << "输入边vi,vj:" << endl;
		int i, j;
		cin >> i >> j;

		EdgeNode *edge = (EdgeNode*)malloc(sizeof(EdgeNode));
		edge->adjvex = j;
		edge->next_edge = G->vertexes[i].first_edge;
		G->vertexes[i].first_edge = edge;
	}
}

void DisplayGraph(Graph *G) {
	EdgeNode *p;
	cout << "序号    信息    相邻顶点" << endl;
	for (int i = 0; i < G->node_num; i++) {
		cout << i << "    " ;
		cout << G->vertexes[i].data << "   ";
		for (p = G->vertexes[i].first_edge; p != NULL; p = p->next_edge)
			cout << p->adjvex << "   ";
		cout << endl;
	}
}

bool visited[MAXNODE];
queue<VertexNode*> q;

void visit(VertexNode * v) {
	cout << v->data << endl;
}

void BFSTraverse(Graph *G) {
	for (int i = 0; i < G->node_num; i++) {
		visited[i] = false;
	}
	for (int i = 0; i < G->node_num; i++) {
		if (!visited[i])
			BFS(G, i);
	}
}

void BFS(Graph *G, int i) {
	visit(&G->vertexes[i]);
	visited[i] = true;
	q.push(&G->vertexes[i]);
	while (!q.empty()) {
		VertexNode *node = q.front();
		q.pop();
		for (EdgeNode *edge = node->first_edge; edge != NULL; edge = edge->next_edge) {
			if (!visited[edge->adjvex]) {
				visited[edge->adjvex] = true;
				visit(&G->vertexes[edge->adjvex]);
				q.push(&G->vertexes[edge->adjvex]);
			}
		}
	}
}

void DFSTraverse(Graph *G) {
	for (int i = 0; i < G->node_num; i++) {
		visited[i] = false;
	}
	for (int i = 0; i < G->node_num; i++) {
		if (!visited[i])
			DFS(G, i);
	}
}


void DFS(Graph *G, int v) {
	visit(&G->vertexes[v]);
	visited[v] = true;
	for (EdgeNode *p = G->vertexes[v].first_edge; p != NULL; p = p->next_edge) {
		if (!visited[p->adjvex])
			DFS(G,p->adjvex);
	}
}