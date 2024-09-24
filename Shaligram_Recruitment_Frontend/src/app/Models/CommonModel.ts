export interface ApiResponse{
    success: boolean;
    data: any;
    message: string;
    totalRecords:number;
}

export interface PageResult{
    search : string,
    page : number, 
    pageSize : number,
    sortColumn : string, 
    sortDirection : string
}
